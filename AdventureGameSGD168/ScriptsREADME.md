# Cat Detective: beginner's guide to every script

Updated September 30, 2026. This guide describes the current prototype. Story details and features can change as the group develops the game.

## Start here

The game has eleven scripts. Each has a different job:

| Script | Its job |
| --- | --- |
| [ButtonController](#1-buttoncontrollercs) | Move between scenes and open the original question-mark dialogue boxes. |
| [Dialogue](#2-dialoguecs) | Show a conversation, type its words, and move through its lines. |
| [CaseProgress](#3-caseprogresscs) | Remember the current task, owned items, and selected item. |
| [AdventureInventory](#4-adventureinventorycs) | Display the inventory and let the player select an item to use. |
| [CaseInteraction](#5-caseinteractioncs) | Decide what happens when the player clicks a character or story object. |
| [EvidenceInventory](#6-evidenceinventorycs) | Remember which clues the player has collected. |
| [EvidencePickup](#7-evidencepickupcs) | Collect an alley clue and prevent collecting it again. |
| [EvidenceReview](#8-evidencereviewcs) | Let the player reread a collected clue's description. |
| [GameMenu](#9-gamemenucs) | Run the start screen, pause menu, and Help screen. |
| [GameAudio](#10-gameaudiocs) | Play background, footsteps, button clicks, cat calls, and the win sound. |
| [UIButtonSound](#11-uibuttonsoundcs) | Give menu and inventory buttons the shared click sound. |

**Items** are things the player uses, such as Fish, Money, and Flashlight. **Case notes** record discoveries, such as the fish wrapping or the shiner's statement. The final discovery adds both a usable **Final evidence** item and a **Hidden evidence** case note.

Progress survives moving between scenes and using Pause/Continue. Starting a fresh Play session, restarting the game, or choosing **Start Game** clears it. There is no save-to-disk system yet. Open **Start_Screen** to begin at the title menu.

## 1. ButtonController.cs

[Open the script](Assets/Scripts/ButtonController.cs)

**Purpose:** handle navigation arrows and the original numbered question-mark buttons.

**Where it goes:** the scene's **Button Controller** GameObject. An arrow's Button component calls this controller through its **On Click()** event.

### Inspector fields

| Field | What to assign |
| --- | --- |
| Dialogue Box 1, 2, 3 | The three original dialogue-box objects in that scene. |
| Button Blocker | The object that blocks scene buttons while dialogue is open. |

### Functions

- `OnArrowPress(string sceneName)` plays the shared arrow sound, then loads the scene named by the button. For example, passing `Streets_One` takes the player to Street 1. The scene name must match the actual scene and be included in the build's scene list. An empty arrow clip slot stays silent.
- `OnQuestionMarkPress(int dialogueBoxNumber)` enables the blocker and opens box 1, 2, or 3. The number comes from the button's On Click event.

The named character buttons now use `CaseInteraction`. The old question-mark system remains in the project, with its template buttons inactive.

**Change here when:** changing how arrows work. To change one arrow's destination, edit its On Click argument in Unity; you usually do not need to edit this script.

## 2. Dialogue.cs

[Open the script](Assets/Scripts/Dialogue.cs)

**Purpose:** display conversation text one letter at a time, then let the player finish or advance the conversation.

**Where it goes:** a dialogue-box GameObject under the scene's **Dialogue Canvas**.

### Inspector fields

| Field | What it means |
| --- | --- |
| Text Component | The TextMeshPro text that displays the conversation. |
| Lines | The lines for a preset conversation. |
| Text Speed | The wait between letters, in seconds. A smaller value types faster. |
| Button Blocker | Stops the player clicking scene buttons behind the conversation. |

For the current story characters, `CaseInteraction` supplies the words through `Show()`. Changing the preset Lines field will not replace those supplied words.

### Functions

- `OnEnable()` starts at the first line whenever the box opens. It clears the text, enables the blocker, and starts typing. If the text reference or lines are missing, it closes the box.
- `Show(string[] newLines)` replaces the conversation and reopens the box so typing starts fresh.
- `LateUpdate()` checks for a released mouse click or finger tap after the UI buttons have run. It ignores taps while paused, taps used on menu buttons, and the frame when the dialogue first opened. This stops Continue from accidentally skipping dialogue.
- `Advance()` finishes a partially typed line. If the line is already complete, it moves to the next line, or closes the box after the last one.
- `TypeLine()` adds one letter, waits, and repeats. This is a **coroutine**: it can pause between letters while the rest of the game continues.
- `OnDisable()` stops typing and turns off the blocker when the box closes.

The private `index` number tracks which line is showing. `openedFrame` remembers when the box opened.

**Change here when:** changing dialogue behavior, such as typing or advancing. Edit most current character dialogue in `CaseInteraction` instead. Mouse interaction has been checked; a phone touch test is still needed.

## 3. CaseProgress.cs

[Open the script](Assets/Scripts/CaseProgress.cs)

**Purpose:** act as shared memory for the current case.

**Where it goes:** nowhere in the Hierarchy. This is a **static class**, so other scripts use it directly. Do not attach it to a GameObject.

### What it remembers

- `CurrentStage`: the current story step, starting at `FindMurder`.
- `SelectedItem`: the name of the item the player has selected. An empty string, `""`, means nothing is selected.
- `items`: a private list of owned item names. Each name can appear once; there are no item quantities yet.

`Stage` is an **enum**, meaning a set of named choices. The current sequence is:

`FindMurder` → `FindShiner` → `TalkDockworker` → `FindCrate` → `SellFish` → `PayShiner` → `GetFlashlight` → `SearchPassage` → `CollectEvidence` → `ReportChief` → `CaseClosed`.

Several story checks compare stages by their order. Reordering these entries can change those checks.

### Functions

- `ResetCase()` clears the items and selection and returns to the first stage. Unity calls it automatically at the start of a session; `GameMenu` also calls it when the player chooses Start Game.
- `HasItem(string itemName)` answers true or false: does the player own this item?
- `AddItem(string itemName)` adds an item if it is not already owned.
- `RemoveItem(string itemName)` removes an item. It also clears the selection if that item was selected.
- `Objective()` returns the instruction for the current stage, such as “Select the money and pay the shiner.” It only supplies text; it does not advance the story.

**Example:** selling the fish removes `Fish`, adds `Money`, and changes `CurrentStage` to `PayShiner`. These changes are requested by `CaseInteraction`.

**Change here when:** editing objective wording or adding a new story stage. A new stage also needs matching behavior in `CaseInteraction`.

## 4. AdventureInventory.cs

[Open the script](Assets/Scripts/AdventureInventory.cs)

**Purpose:** show the player's usable items and let them select one, then click a target.

**Where it goes:** the shared [Evidence_UI prefab](Assets/Prefabs/Evidence_UI.prefab). This prefab supplies the inventory to the main scenes.

### Inspector fields

| Field | What to assign |
| --- | --- |
| Panel | The inventory panel to open and close. |
| Evidence | The `EvidenceInventory` component for the case notes. |
| Fish / Money / Flashlight / Final Evidence Button | Each usable item's button. |
| Cancel Button | The object used to cancel an item selection. |
| Selection Text | The label showing which item is selected. |
| Empty Text | The message shown when no usable items are owned. |
| Objective Text | The current task label. |

These references are already connected on the shared prefab. Keep its **Evidence_Panel** inactive in Edit mode so the game starts with the panel closed.

### Functions

- `Start()` calls `Refresh()` when the component starts.
- `OpenInventory()` refreshes the contents and opens the panel. The Inventory button calls this.
- `SelectItem(string itemName)` checks ownership, remembers the selection, closes the panel, and updates the display. Each item button passes its item name.
- `CancelSelection()` clears the selected item and refreshes the display. It does not remove the item from the inventory.
- `Refresh()` shows owned item buttons, hides unowned ones, updates the empty message and selection label, updates the objective, and asks `EvidenceInventory` to update its display.

Selecting an item does not use it immediately. For example: select Fish, then click the fish seller. `CaseInteraction` decides whether that trade is allowed.

The line containing `selected ? ... : ...` is a short way to write an if/else: if an item is selected, show its name; otherwise, show blank text.

**Change here when:** adding a usable item's button or changing inventory selection behavior. The current inventory has four fixed item buttons; adding a name to `CaseProgress` alone will not create another button.

## 5. CaseInteraction.cs

[Open the script](Assets/Scripts/CaseInteraction.cs)

**Purpose:** run the story interactions. It checks the current task and selected item, then gives dialogue, rewards, or the next objective.

**Where it goes:** a character or story-object button. Its Button component's On Click event calls that object's `CaseInteraction.Interact()`.

### Inspector fields

| Field | What it means |
| --- | --- |
| Role | Chooses what kind of character or object this button represents. |
| Dialogue | The `Dialogue` component used for its conversation. |
| Bag | The shared `AdventureInventory` component to refresh afterward. |
| Evidence | The `EvidenceInventory` component used when recording/checking clues. |
| Hidden Evidence | The hidden object to reveal for the Darkness interaction. |
| Ending Panel | The ending display used by the Chief interaction. |
| Hint | An ordered list of dialogue lines used when Role is Hint. |
| Success Sound | Optional Audio Source, already connected on Search_Darkness and Hidden_Evidence. Assign a clip on that source. |
| Play Cat Call | Enabled for living cat characters, including the street cats on Streets 1 and 2. Plays a random call when clicked. Leave off on objects and the deceased cat. |

The Hidden Evidence and Ending Panel fields are for the roles that use them. They do not need to be filled on every character.

### Roles and their functions

| Role | Function | What happens |
| --- | --- | --- |
| Murder | `InspectMurder()` | Records the examined crime scene and directs the player to the shiner. |
| Shiner | `TalkShiner()` | Requests a coin. Paying with selected Money spends it, records testimony, and directs the player to the chief. |
| Dockworker | `TalkDockworker()` | Offers the misplaced-crate job after the player has spoken to the shiner. |
| DeliveryCrate | `FindCrate()` | Rewards Fish when the crate job is active. Repeated clicks do not give extra fish. |
| FishSeller | `TalkFishSeller()` | Trades selected Fish for Money when the player reaches that task. |
| Chief | `TalkChief()` | Gives the flashlight after the shiner's tip. Later checks the presented final evidence and ends the case. |
| Darkness | `SearchDarkness()` | Reveals the hidden evidence when the player uses the selected Flashlight at the correct stage. The flashlight is kept. |
| HiddenEvidence | `CollectFinalEvidence()` | Adds the Final evidence item and Hidden evidence case note, then directs the player back to the chief. |
| Hint | Calls `Say(hint)` | Displays the Hint text entered in the Inspector. |

### Other functions

- `Start()` restores the hidden object's visibility and ending panel from the current stage when entering a scene.
- `Interact()` plays a cat call when Play Cat Call is enabled, chooses the role's function, then refreshes the inventory. It stops accepting these interactions once the case is closed.
- `Say(string[] line)` gives the full ordered conversation to the dialogue box. Each entry is one line; the player advances through them.
- `PlaySuccessSound()` plays the connected source's clip if one is assigned. Successful flashlight use and final-clue collection call it; wrong items and repeat interactions do not.
- `WrongItem(string expected)` checks whether a different item is selected. It displays a response and keeps the item when it is unsuitable. With no item selected, normal conversation can proceed; a trade still requires the correct selected item.

Most roles reject unsuitable selections. If an item is still selected and the player wants to investigate normally, use **Cancel item**.

### What currently ends the game

The player must reach `ReportChief`, own and select `Final evidence`, and click the chief. The chief also checks that **Fish wrapping** and **Delivery receipt** were collected. If either is missing, the player can go back for it. If both are present, the stage becomes `CaseClosed` and the ending panel opens. `GameAudio.PlayWin()` stops the background and cat audio and plays Game_Win once.

The actual murderer and identifying object are still story placeholders. This script currently handles the puzzle sequence, not a finished explanation of the murder.

**Change here when:** writing character dialogue, changing a trade, adding a story interaction, or changing the ending requirements.

## 6. EvidenceInventory.cs

[Open the script](Assets/Scripts/EvidenceInventory.cs)

**Purpose:** remember discoveries and support the clue display and objective text.

**Where it goes:** the shared inventory UI in the main scenes. The older **AlexTestScene** has its own evidence manager setup.

### Inspector fields

- **Evidence List Text:** the text object that holds the collected-clue list. The duplicate list is hidden in the main UI, but this reference is still required by the current code. Do not clear it just because the list is invisible.
- **Objective Text:** the task label to update.
- **Use Case Objectives:** enabled for the main game's story objectives. Disabled in the older test scene to keep its two-clue instructions.

The private static `collectedEvidence` list is shared across scenes during a game session. It is separate from the usable-item list in `CaseProgress`.

### Functions

- `ResetEvidence()` clears collected clues at the beginning of a fresh session and when `GameMenu` starts a new case.
- `Start()` updates the display when the component starts.
- `HasEvidence(string evidenceName)` answers whether a named clue has been collected.
- `AddEvidence(string evidenceName)` adds the name if needed and updates the display. It returns false if Evidence List Text is missing; otherwise it returns true, including when the clue was already recorded. Duplicate names are not added twice.
- `UpdateDisplay()` rebuilds the list text and updates the objective. With Use Case Objectives enabled, it gets the wording from `CaseProgress.Objective()`. Otherwise, it uses the original zero/one/two-clue messages.

The `foreach` loop means “go through each collected clue.” It appends the clue's name and `\n`, which starts a new line. The text is cleared before the loop so each refresh does not repeat the old list.

**Change here when:** changing how clues are stored or how the original test scene's clue count is displayed. Edit main story objectives in `CaseProgress`.

## 7. EvidencePickup.cs

[Open the script](Assets/Scripts/EvidencePickup.cs)

**Purpose:** collect a clickable clue once and mark it as collected.

**Where it goes:** the red **Fish_Wrapping_Clue** and **Delivery_Receipt_Clue** buttons. The script requires Button and Audio Source components. Assign a pickup clip on the object's Audio Source; an empty clip is allowed.

### Inspector fields

| Field | What it means |
| --- | --- |
| Clue Text | The label on the red pickup button. |
| Inventory | The `EvidenceInventory` component that records this clue. |
| Evidence Name | This object's clue name, such as `Fish wrapping` or `Delivery receipt`. |
| Description Text | Where the clue's explanation will be written. |
| Evidence Description | The explanation of this particular clue. |
| Inventory Button | An optional review button to enable and connect directly. The shared UI also uses `EvidenceReview`. |

Both clues use the same script. Each component has its own Inspector values. `"Fish wrapping"` in the code is the initial default, not a separate field for every possible clue.

### Functions

- `OnEnable()` checks whether this clue is already recorded. It restores the collected label and disabled pickup button when the player returns to the scene. If an Inventory Button is assigned, it also sets that button's availability and connects `ShowDescription()` to its click.
- `Awake()` remembers the attached Audio Source and sets it to 2D, with looping and autoplay off.
- `CollectEvidence()` stops if this pickup was already collected. Otherwise, it records the clue, plays the pickup clip if assigned, enables an assigned review button, updates the description and label, and disables the pickup button. Sound plays only after recording succeeds.
- `ShowDescription()` writes this clue's description only after collection.
- `OnDisable()` removes the review-button click connection it added earlier.

The red pickup button's On Click event calls `CollectEvidence()`. Collecting a clue updates the information **without opening the inventory panel**.

**Change here when:** changing pickup behavior. To change one clue's name or description, select its GameObject in the Hierarchy and edit its component in the Inspector. Selecting the `.cs` file in the Project panel does not edit that scene object's settings.

## 8. EvidenceReview.cs

[Open the script](Assets/Scripts/EvidenceReview.cs)

**Purpose:** let the player read collected case notes from the inventory in any main scene.

**Where it goes:** each case-note button inside the shared inventory panel. It requires a Button component.

### Inspector fields

- **Inventory:** the `EvidenceInventory` component to check.
- **Evidence Name:** the exact name of the recorded clue.
- **Description Text:** the text area that shows the explanation.
- **Evidence Description:** the words to show when this button is clicked.

### Functions

- `OnEnable()` checks whether the clue is collected. It enables or grays out the button accordingly and connects its click to `ShowDescription()`.
- `ShowDescription()` checks ownership again and displays the description. It does not collect or spend anything.
- `OnDisable()` removes its click connection when the button/panel closes, preventing repeated connections as the panel is reopened.

This script connects its own click action. You do not need an additional Inspector On Click entry for `ShowDescription()` on these review buttons.

**Change here when:** changing review-button behavior. Edit a button's Evidence Description in the prefab to change its wording. For alley clues, keep that description consistent with the matching `EvidencePickup` description.

## 9. GameMenu.cs

[Open the script](Assets/Scripts/GameMenu.cs)

**Purpose:** open the pause and Help panels, resume the case, and move between the title screen and gameplay.

**Where it goes:** the shared [Game_Menu prefab](Assets/Prefabs/Game_Menu.prefab). One instance is already in **Start_Screen** and each of the ten gameplay scenes. The older AlexTestScene is unchanged.

### Inspector fields

| Field | What it means |
| --- | --- |
| Is Start Screen | Enabled only on the Start_Screen instance. Shows the title menu instead of the Pause button. |
| Start Panel | The title, Start Game button, and Help button. |
| Pause Button | The on-screen button below Inventory. |
| Pause Panel | Contains Continue, Help, and Quit. |
| Help Panel | Contains the controls, objective, and Back button. |
| Start Scene Name | `Start_Screen`, loaded by Quit. |
| Game Scene Name | `Office (Start)`, loaded by Start Game. |

These fields and button events are already connected. Edit the shared prefab to change the layout or Help wording for every scene at once.

### Functions

- `Awake()` shows the right starting controls and hides the other panels.
- `PauseGame()` remembers the current game speed, sets `Time.timeScale` to zero, and opens Pause. The full-screen panel blocks clicks on gameplay behind it.
- `ContinueGame()` closes the menu and restores the previous speed. It preserves the current scene, inventory, selected item, and open conversation.
- `ShowHelp()` remembers whether Help was opened from Pause, then displays the instructions. A paused case stays paused.
- `BackFromHelp()` returns to Pause or the title panel, depending on where Help was opened.
- `QuitToStart()` restores the game speed, resumes the background if the win stopped it, and loads Start_Screen.
- `StartGame()` clears the case and collected clues, ensures the background is playing, then loads Office (Start). This starts a new case rather than resuming the old one.
- `OnDisable()` restores the game speed if the menu is removed while paused, preventing a scene change from leaving the game frozen.
- `BlocksDialogueInput` tells Dialogue to ignore input while paused and on the frame a menu button was clicked. `ResetMenuInput()` clears that remembered frame when a Play session starts.

The `helpCameFromPause` true/false variable is how Back knows where to go. Help is a panel in the same scene, so reading it does not unload the player's current location.

**Change here when:** changing menu behavior. To edit instructions, open **Game_Menu > Help_Panel** in Prefab Mode and change its TextMeshPro text. See [MenuNotes.md](MenuNotes.md) for setup and a quick check.

## 10. GameAudio.cs

[Open the script](Assets/Scripts/GameAudio.cs)

**Purpose:** manage the sounds shared across scenes and switch from background audio to the win sound when the case is solved.

**Where it goes:** the root of **Assets/Resources/Game_Audio.prefab**. The game creates this prefab automatically when Play starts. You do not need to put it in each scene.

### Inspector fields

| Field | What it means |
| --- | --- |
| Background Source | Already connected to the Background_Audio child. Put your background clip on that child's Audio Source. |
| Arrow Source | Connected to Arrow_Audio, using Arrow_FootSteps. |
| Button Source | Connected to Button_Audio, using Button_Click. |
| Cat Source | Connected to Cat_Audio. It plays the clip chosen from Cat Calls. |
| Win Source | Connected to Win_Audio, using Game_Win. |
| Cat Calls | The six Cat_Call clips from Assets/SOUNDS. |

The background source loops at volume 0.445. Arrow and button sources use 0.75, cat calls use 0.65, and the win source uses 0.7. Effects do not loop. All are 2D with Play On Awake off because code controls playback. Empty clip slots are safe. The background's import setting uses Streaming for this long track.

### Functions

- `ResetAudio()` clears the remembered instance at the beginning of a Play session.
- `CreateAudio()` loads the Game_Audio prefab from Resources and creates it before the first scene loads. The attribute above the method tells Unity to run it automatically.
- `Awake()` removes duplicate audio objects, keeps the original alive with `DontDestroyOnLoad`, and starts the background clip if one is assigned.
- `PlayArrow()` plays the arrow source's clip if one is assigned. `ButtonController` calls this before loading the next scene.
- `PlayButton()` plays Button_Click through the shared source. `UIButtonSound` calls it for menu and inventory buttons, including ones that close their own panel.
- `PlayCat()` randomly chooses one of the six cat calls. It replaces the previous call instead of stacking several voices. It does nothing while the win audio state is active.
- `PlayWin()` stops the background and cat call, then starts Game_Win once. The private `winStarted` true/false variable prevents repeated triggering.
- `PlayBackground()` clears the win state, stops win audio, and starts the background if it is not already playing. Existing background playback is not restarted when moving into a new case.
- `OnDestroy()` clears the remembered instance if that object is removed.

The private static `instance` field remembers the one audio object shared by the whole game. It prevents two copies of the background track. Music continues through scenes, Pause, and Help, stops at the chief's Case_Closed_Panel, and stays off after the win clip finishes. Quit returns to the title and resumes the background.

**Change here when:** changing shared audio behavior. To choose sounds or adjust their volume, edit the prefab's child Audio Sources instead. Keep the prefab name and Resources location unchanged. See [AudioNotes.md](AudioNotes.md) for the exact clip slots, including clue and flashlight sounds.

## 11. UIButtonSound.cs

[Open the script](Assets/Scripts/UIButtonSound.cs)

**Purpose:** play the ordinary click sound when an available menu or inventory button is clicked or tapped.

**Where it goes:** buttons in the shared Game_Menu and Evidence_UI prefabs. The older AlexTestScene's inventory controls also have it. It requires a Button component and has no Inspector fields to fill in.

- `Awake()` remembers the attached Button.
- `OnEnable()` connects the button's click to `GameAudio.PlayButton()`.
- `OnDisable()` removes that connection so reopening a panel does not accumulate extra listeners.

It uses the persistent Button_Audio source, so closing a panel or changing scenes does not cut the click off. Add this component to future ordinary UI buttons. Do not add it to arrows, cat interactions, or physical clue pickups that already trigger their own sounds.

**Change here when:** changing how UI buttons request their sound. Choose the actual clip and volume on the Game_Audio prefab's Button_Audio child.

## Names that must match

The code currently identifies items and clues by exact text, including capitalization and spaces. A visible button label can be friendlier, but its stored name must match the code.

| Type | Exact names used by the code |
| --- | --- |
| Usable items | `Fish`, `Money`, `Flashlight`, `Final evidence` |
| Case notes | `Fish wrapping`, `Delivery receipt`, `Crime scene examined`, `Shiner testimony`, `Hidden evidence` |

For example, the label **Delivery Receipt** can display on a button while its Evidence Name field remains `Delivery receipt`.

## How the scripts work together: selling a fish

1. The player opens Inventory. `AdventureInventory` shows Fish because `CaseProgress` says it is owned.
2. The player selects Fish. `AdventureInventory` stores the selection and closes the panel.
3. The player clicks the fish seller. That button calls `CaseInteraction.Interact()`.
4. `TalkFishSeller()` checks the stage, ownership, and selection. It asks `CaseProgress` to remove Fish, add Money, and move to `PayShiner`.
5. `Dialogue` displays the seller's response. `AdventureInventory.Refresh()` updates the available items and task text.

## A few code terms in plain language

| Term | Meaning here |
| --- | --- |
| GameObject | An object in Unity's Hierarchy, such as a button or dialogue panel. |
| Component | A part attached to an object, such as a Button or one of these scripts. |
| `[SerializeField]` | Lets Unity save a private field and show it in the Inspector. |
| `TMP_Text` / `TextMeshProUGUI` | References to text components used to display words. |
| `string` / `bool` / `int` | Text / true-or-false / a whole number. |
| `null` | No object reference is assigned. |
| `return` | Stop this function now; sometimes also send back an answer. |
| `=` / `==` | Assign a value / check whether two values match. |
| `!` / `&&` / `\|\|` | Not / both conditions must be true / either condition may be true. |
| `SetActive(false)` | Hide/deactivate a GameObject. |
| `interactable = false` | Leave a button visible but prevent clicking it. |

For the complete play sequence, see [CaseAdventureNotes.md](CaseAdventureNotes.md). For the shared UI setup, see [InventoryNotes.md](InventoryNotes.md). For scene connections, see [NavigationNotes.md](NavigationNotes.md).

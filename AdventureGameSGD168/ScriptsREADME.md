# Cat Detective: beginner's guide to every script

Updated September 23, 2026. This guide describes the current prototype. Story details and features can change as the group develops the game.

## Start here

The game has eight scripts. Each has a different job:

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

**Items** are things the player uses, such as Fish, Money, and Flashlight. **Case notes** record discoveries, such as the fish wrapping or the shiner's statement. The final discovery adds both a usable **Final evidence** item and a **Hidden evidence** case note.

Progress survives moving between scenes. Starting a fresh Play session or restarting the game clears it. There is no save-to-disk system yet.

## 1. ButtonController.cs

[Open the script](Assets/ButtonController.cs)

**Purpose:** handle navigation arrows and the original numbered question-mark buttons.

**Where it goes:** the scene's **Button Controller** GameObject. An arrow's Button component calls this controller through its **On Click()** event.

### Inspector fields

| Field | What to assign |
| --- | --- |
| Dialogue Box 1, 2, 3 | The three original dialogue-box objects in that scene. |
| Button Blocker | The object that blocks scene buttons while dialogue is open. |

### Functions

- `OnArrowPress(string sceneName)` loads the scene named by the button. For example, passing `Streets_One` takes the player to Street 1. The scene name must match the actual scene and be included in the build's scene list.
- `OnQuestionMarkPress(int dialogueBoxNumber)` enables the blocker and opens box 1, 2, or 3. The number comes from the button's On Click event.

The named character buttons now use `CaseInteraction`. The old question-mark system remains in the project, with its template buttons inactive.

**Change here when:** changing how arrows work. To change one arrow's destination, edit its On Click argument in Unity; you usually do not need to edit this script.

## 2. Dialogue.cs

[Open the script](Assets/Dialogue.cs)

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
- `Update()` checks each frame for a left mouse click. It ignores the frame when the box first opened, so the opening click does not also skip the text.
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

- `ResetCase()` clears the items and selection and returns to the first stage when a new game session starts. Unity calls it automatically.
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
| Hint | The words used when Role is Hint. |

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
- `Interact()` chooses the role's function, then refreshes the inventory. It stops accepting these interactions once the case is closed.
- `Say(string line)` gives one line of text to the dialogue box.
- `WrongItem(string expected)` checks whether a different item is selected. It displays a response and keeps the item when it is unsuitable. With no item selected, normal conversation can proceed; a trade still requires the correct selected item.

Most roles reject unsuitable selections. If an item is still selected and the player wants to investigate normally, use **Cancel item**.

### What currently ends the game

The player must reach `ReportChief`, own and select `Final evidence`, and click the chief. The chief also checks that **Fish wrapping** and **Delivery receipt** were collected. If either is missing, the player can go back for it. If both are present, the stage becomes `CaseClosed` and the ending panel opens.

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

- `ResetEvidence()` clears collected clues at the beginning of a fresh session.
- `Start()` updates the display when the component starts.
- `HasEvidence(string evidenceName)` answers whether a named clue has been collected.
- `AddEvidence(string evidenceName)` adds the name if needed and updates the display. It returns false if Evidence List Text is missing; otherwise it returns true, including when the clue was already recorded. Duplicate names are not added twice.
- `UpdateDisplay()` rebuilds the list text and updates the objective. With Use Case Objectives enabled, it gets the wording from `CaseProgress.Objective()`. Otherwise, it uses the original zero/one/two-clue messages.

The `foreach` loop means “go through each collected clue.” It appends the clue's name and `\n`, which starts a new line. The text is cleared before the loop so each refresh does not repeat the old list.

**Change here when:** changing how clues are stored or how the original test scene's clue count is displayed. Edit main story objectives in `CaseProgress`.

## 7. EvidencePickup.cs

[Open the script](Assets/Scripts/EvidencePickup.cs)

**Purpose:** collect a clickable clue once and mark it as collected.

**Where it goes:** the red **Fish_Wrapping_Clue** and **Delivery_Receipt_Clue** buttons. The script requires a Button component.

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
- `CollectEvidence()` stops if this pickup was already collected. Otherwise, it records the clue, enables an assigned review button, updates the description and label, and disables the pickup button.
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

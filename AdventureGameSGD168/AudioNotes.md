# Cat Detective: audio setup

Updated September 30, 2026. The downloaded clips are connected; no additional clip assignments are needed for the existing scenes.

| Sound | Clip | When it plays |
| --- | --- | --- |
| Background | BackGround_Sound | Starts on the title screen, loops across scenes and Pause/Help, and stops when the chief closes the case. |
| Navigation | Arrow_FootSteps | Clicking or tapping a navigation arrow. The sound can finish after the destination loads. |
| UI buttons | Button_Click | Start, Pause, Continue, Help, Back, Quit, inventory controls, and available clue-review buttons. |
| Cats | Cat_Call_1 through Cat_Call_6 | A random call when interacting with the shiner, dockworker, fish seller, chief, or the street cats on Streets 1 and 2. The murdered cat does not meow. |
| Flashlight | FlashLight_Click | Successfully using the selected flashlight on the dark area in Dark_Passage. Selecting it in the inventory only makes the normal UI click. |
| Physical clues | PageTurn | First pickup of fish wrapping, delivery receipt, or hidden evidence. Reviewing clues does not replay their pickup sound. |
| Win | Game_Win | Plays once when complete evidence is presented to the chief and Case_Closed_Panel opens. |

The win display is the existing **Case_Closed_Panel in Cheifs_Office**, not Dark_Passage. Using the flashlight and finding the final clue do not end the game. At the win display, background and cat audio stop. The background stays off after the win clip finishes. Choosing Quit to return to the title restarts it; Start Game begins a fresh case.

## Adjusting sounds and volume

Stop Play mode before making changes. In the Project panel, double-click **Assets > Resources > Game_Audio**. Select one of its children and edit its **Audio Source**:

- **Background_Audio:** background clip; loops; volume 0.25.
- **Arrow_Audio:** footsteps; no loop; volume 0.75.
- **Button_Audio:** UI click; no loop; volume 0.75.
- **Cat_Audio:** shared cat voice; no loop; volume 0.65. Its clip is chosen during play from the root GameAudio component's **Cat Calls** list.
- **Win_Audio:** win clip; no loop; volume 0.7.

The clip slot is labeled **Audio Generator** in this project's Unity Inspector. Keep **Play On Awake off**; the scripts control when each clip starts. All sources are 2D. The long background clip uses **Streaming** in its import settings to avoid decompressing the whole track into memory.

Local sources are on **Fish_Wrapping_Clue** and **Delivery_Receipt_Clue** in Alley_Murder, and **Search_Darkness** and **Hidden_Evidence** in Dark_Passage. The older AlexTestScene's clue sources are also assigned. Save the relevant scene or prefab after editing.

Game_Audio creates itself when a Play session starts. Keep its name and Resources location unchanged; do not add a copy to every scene. Editing its temporary copy under DontDestroyOnLoad during Play does not save changes.

## Adding another interactive cat or button

- On a new character using **CaseInteraction**, enable **Play Cat Call** in the Inspector. Leave it off for objects, signs, and the deceased cat.
- Add **UIButtonSound** to a new ordinary menu or inventory button. It handles its own click listener. Do not add it to navigation arrows or clue-pickup/character targets that already have their own sounds.
- New EvidencePickup objects require an Audio Source. Assign a short pickup clip there and leave autoplay and looping off.

The code handles mouse clicks and finger taps through the same existing button events. New sounds do not change item requirements or story progression. See [ScriptsREADME.md](ScriptsREADME.md) for the code explanations.

## Verification

After recovering Unity's generated Library from the system crash, **74 Play mode checks passed with 0 errors** using the actual imported clips. They checked button callbacks and playback state for music continuity, menu/inventory clicks, footsteps across a scene change, living-cat calls, clue collection, successful flashlight use, the win transition, and restarting a case. These checks do not replace listening to the mix or testing physical taps on the Android tablet.

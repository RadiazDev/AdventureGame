# Cat detective: playable case prototype

Updated September 23, 2026. Open **Office (Start)** and press Play for a fresh game. Characters and objects use labeled red buttons for now. No new artwork was generated.

## How to play

Click/tap a character to talk. Tap to finish a typing line, then tap again to close it. The existing dialogue boxes are reused.

Open **Inventory**, select an item, then tap the character or object you want to use it on. Selecting an item closes the inventory and shows **Using: [item]** along the bottom. **Cancel item** returns to ordinary investigating. Using an item on the wrong target keeps it. Case-note buttons become available as evidence is collected; tapping them rereads the description. Collecting a clue does not automatically open the inventory.

## Walkthrough

1. Office -> Street 1 -> Murder Alley. Tap **Examine alley cat**. Collect **Fish Wrapping** and **Delivery Receipt**.
2. Go right to Dumpster Alley. Talk to the **Shoe / claw shiner**. He wants a coin for his information.
3. Return through Murder Alley to Street 1, then down to the Dock. Talk to the **Dockworker** and tap the **Misplaced crate**. He rewards you with a fish.
4. Dock -> Street 3 -> Fish Market. Open Inventory, select **Fish**, then tap the **Fish seller**. The fish is exchanged for **Money**.
5. Fish Market -> Street 3 -> Street 2 -> Dumpster Alley. Select **Money** and tap the **Shiner**. The coin is spent; his witness statement is added to the case notes.
6. Dumpster Alley -> Street 2 -> Chief's Office. Talk to the **Chief** to receive a **Flashlight**.
7. Leave the Chief's Office for Street 3, then go right into **Dark Passage**. Select **Flashlight**, then tap **Search the darkness**. Close the dialogue and tap **Collect hidden evidence**. The flashlight stays in your inventory.
8. Dark Passage -> Street 3 -> Street 2 -> Chief's Office. Select **Final evidence**, then tap the **Chief**. If both original alley clues are also collected, the case ends with **Case closed. Purr-petrator caught!**

You can enter Dark Passage before getting the flashlight, but its evidence remains hidden. Missing an alley clue does not trap the player: the chief asks for it, and the player can return to collect it.

## Simple code guide

- **CaseProgress.cs**: one current objective, a list of owned items, and the selected item. These values survive scene changes and reset when a new game starts.
- **AdventureInventory.cs**: opens the panel, shows owned item buttons, selects/cancels an item, and updates the objective text.
- **CaseInteraction.cs**: a role chosen in the Inspector tells a character/object what to do. Each role has a short method using ordinary conditions to check progress and the selected item. Conversation text is in these methods.
- **EvidenceInventory / EvidencePickup / EvidenceReview**: retain the existing clue collection and review behavior. The shared prefab uses the new case objectives; AlexTestScene retains its old two-clue objective.
- **Dialogue.cs**: restarts typing whenever a conversation opens, including repeat conversations. Its blocker stops clicks reaching the scene behind the dialogue.

Edit **Assets/Prefabs/Evidence_UI.prefab** for shared inventory layout. Keep **Evidence_Panel** inactive in Edit mode. The old question-mark buttons remain inactive in the scene templates; the named red buttons are the connected prototype interactions.

## Still to decide

The killer and the exact final evidence are deliberately unnamed. The hidden-object button and its text are placeholders until the group agrees how the object identifies the murderer. This is a working puzzle sequence, not the final mystery script. Character art, item icons, sound, and final dialogue can be added later. There is no save-to-disk system: restarting Play or closing the game clears progress.

## Validation

Unity 6000.3.17f1 compiled the scripts. A temporary Play mode check passed 206 assertions with zero errors across the whole case chain, including actual UI raycast reachability and button-event calls, repeated conversations/rewards, wrong-item handling, item persistence between scenes, flashlight selection, and the chief's ending requirements. The check loaded some scenes directly; it was not a phone playthrough.

Mouse checks in the 16:10 Game view also covered inventory selection and wrong-item dialogue. A temporary preview supplied the flashlight and earlier clues to check the final portion by mouse: revealing and collecting the hidden evidence, following arrows back to the chief, selecting Final evidence, and seeing the ending. That preview state was not saved. No phone build or touch-device test has been run. Temporary setup and validation scripts are removed from Assets after use.

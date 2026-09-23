# Inventory and case notes

Every main scene uses **Assets/Prefabs/Evidence_UI.prefab**. Its **Inventory** button opens a panel with two sections:

- **Case notes:** Fish Wrapping, Delivery Receipt, Crime scene, Witness statement, and Hidden evidence. Buttons are disabled until their matching evidence is collected. Tap an available button to reread it. The duplicate text list remains hidden.
- **Items:** Fish, Money, Flashlight, and Final evidence appear only while owned. Tap an item to select it and close the panel, then tap a character/object to use it. The bottom label shows the selection; **Cancel item** clears it.

Fish is traded for money; money is spent on the shiner's information. The flashlight is retained after use. Wrong targets do not consume items. Items, evidence, objectives, and selections persist while changing scenes. Starting a fresh game resets them; saving to disk is not implemented.

Collecting the red alley clues still leaves the inventory closed. Returning to Murder Alley keeps collected pickup buttons disabled. The original **AlexTestScene** remains separate with its older two-clue UI.

## Editing

Change the shared prefab to update the inventory in every main scene. Keep **Evidence_Panel** inactive in Edit mode. Scene references and button events are already assigned.

For the original clues, use exactly **Fish wrapping** and **Delivery receipt** in Evidence Name fields. Pickup and review descriptions should agree. Their names must match the checks in **CaseInteraction.TalkChief**. New case-note names are **Crime scene examined**, **Shiner testimony**, and **Hidden evidence**.

The scripts and full walkthrough are explained in **CaseAdventureNotes.md**. Adding a future usable item currently means adding its button/reference in **AdventureInventory**, its interaction in **CaseInteraction**, and any relevant objective in **CaseProgress**. This deliberately small prototype uses a fixed item set.

## Validation

The September 23 Play mode check passed 206 assertions with zero errors across the investigation and item trades. It checked item selection, click targets, repeated rewards, wrong targets, scene persistence, and final evidence requirements. Mouse checks covered the shared inventory and dialogue. No phone build or touch test has been run.

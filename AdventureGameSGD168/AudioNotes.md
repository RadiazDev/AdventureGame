# Cat Detective: adding your sounds

Updated September 30, 2026.

The code and Audio Sources are connected. You only need to import your clips, assign them, and save. Empty clip slots stay silent without stopping the game.

## 1. Import the downloads

1. Stop Play mode in Unity before changing anything.
2. Drag your downloaded audio files into **Assets > SOUNDS** in the Project panel.
3. Wait for Unity to import them.

You can reuse one pickup clip for all three physical clues. The fish-wrapping clue already has your **PageTurn** clip assigned; that assignment was preserved.

## 2. Background track and all navigation arrows

1. In the Project panel, open **Assets > Resources**.
2. Double-click **Game_Audio** to open the prefab.
3. In its Hierarchy, select **Background_Audio**.
4. In the Inspector, find its **Audio Source** component and drag your background clip into **Audio Generator** (the top audio slot currently showing None in your Unity Inspector).
5. Select **Arrow_Audio** and put your arrow-click clip in that object's Audio Source field.
6. Save the prefab with **Ctrl+S**, then use the back arrow to leave Prefab Mode.

The background source already loops at volume **0.25**. The arrow source starts at **0.75** and does not loop. Adjust those volumes after listening to your clips. Keep **Play On Awake off** on both: the code starts the background and plays arrows when needed.

**Assign the arrow clip once.** All existing navigation arrows call the shared source, which survives scene changes so the click can finish. The background also continues across scenes, Pause, Help, and returning to the start screen.

You do not need to add this prefab to any scene. `GameAudio` automatically creates one copy when Play starts, even when starting directly in a gameplay scene. Keep its name **Game_Audio** and its location inside **Assets/Resources**.

## 3. Clue pickups and the flashlight

Open each scene from **Assets > Scenes**, select the named object in the **Hierarchy**, and assign your clip in that object's **Audio Source > Audio Generator** field.

| Scene | Object | Clip to assign |
| --- | --- | --- |
| Alley_Murder | Fish_Wrapping_Clue | Pickup sound; PageTurn is already assigned. |
| Alley_Murder | Delivery_Receipt_Clue | Pickup sound. |
| Dark_Passage | Search_Darkness | Flashlight switch or reveal sound. |
| Dark_Passage | Hidden_Evidence | Pickup sound for the final clue. |

Expand the Hierarchy or search it by the object's name. Hidden_Evidence starts inactive, but you can still select it and edit its Audio Source. Save each scene with **Ctrl+S** after assigning the clips.

The **Success Sound** references on the two Dark_Passage objects are already connected to their Audio Sources. Put your downloaded clip in the **Audio Source** component; you do not need to change those script references.

New effect sources are set to **2D**, **Loop off**, **Play On Awake off**, and volume **0.8**. The existing fish-wrapping source keeps its original volume. Leave autoplay and looping off for these short effects.

The older **AlexTestScene** also has Audio Sources on its two clue buttons. Assign clips there separately if you still use that test scene.

## What triggers each sound

- Background: starts automatically when the game starts, then loops continuously.
- Navigation arrow: plays when the player clicks or taps an arrow, before the destination loads.
- Alley clue: plays after the clue is successfully recorded. It does not replay when reviewing the clue or returning to the scene.
- Flashlight: plays when the selected flashlight successfully reveals the hidden evidence. Wrong items, missing flashlight, and repeated clicks do not play this success sound.
- Hidden evidence: plays when the player first collects the final clue.

Mouse clicks and finger taps use the same existing button actions, so they trigger the same sounds.

## Avoid losing your assignments

Make these changes **outside Play mode**. The Game_Audio object under DontDestroyOnLoad during Play is a temporary copy; editing that copy will not save your clips. Edit the prefab in the Project panel instead.

If you hear nothing, check that the clip is assigned, the source's Mute checkbox is off, its Volume is above zero, and the Game view's audio mute setting and device volume allow sound.

## Useful optional sounds for later

These are suggestions, not additional systems installed in this change:

- A soft click for opening the inventory or selecting menu buttons.
- A coin jingle for selling the fish or paying the shiner.
- A short celebration sound when the chief closes the case.

## Code and verification

`GameAudio.cs` owns the two shared sources. `ButtonController.cs` requests the arrow sound. `EvidencePickup.cs` uses the Audio Source on its clue. `CaseInteraction.cs` plays the connected source after successful flashlight use or hidden-evidence collection. See [ScriptsREADME.md](ScriptsREADME.md) for the function explanations.

Unity Play mode verification passed **79 checks with 0 errors**, covering scene changes across all 11 build scenes, music continuity, arrow playback, clue collection/review, flashlight conditions, pause/help, and starting a new case. The checks used temporary silent clips to verify playback state; those clips were not saved to the project. Listening to your chosen clips and testing the final Android build remain to be done after you assign the audio.

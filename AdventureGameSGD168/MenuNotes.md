# Start, Pause, and Help

Added September 28, 2026.

## Try it

1. Open `Assets/Scenes/Start_Screen.unity` and press Play.
2. Choose **Help** to read the controls and murder-case objective. **Back** returns to the title screen.
3. Choose **Start Game**. The game opens **Office (Start)** with a fresh case.
4. Click or tap **Pause**, below the Inventory button at the upper right.
5. **Continue** resumes the same location and progress. **Help** opens the instructions, and **Back** returns to the pause choices.
6. **Quit** returns to the title screen. Choosing **Start Game** again clears the previous case, including items, selected item, objectives, and collected clues.

There is no save file or Continue option on the title screen. Use Pause's **Continue** when you want to keep playing the current case.

## Editing the words or layout

The title screen was updated September 30, 2026 with **A Fishy Alibi**, **Created by Ryan Diaz and Alex Freeman**, and the cat-detective alley background. In `Game_Menu.prefab`, expand **Start_Panel** to edit **Title** or **Tagline**. Its Image uses `Assets/Sprites/Title_Screen_BG.png`. **Start_Footer** is disabled so the old mouse/touch hint does not appear. Start Game and Help keep their existing actions and button sounds.

Open `Assets/Prefabs/Game_Menu.prefab` in Prefab Mode. Expand **Help_Panel**, then select **Objective**, **Controls**, or **Inventory_Help** and edit the TextMeshPro text. Temporarily enable Help_Panel to preview it, then turn it off before saving. The shared prefab updates all game scenes.

The simple menu controller is `Assets/Scripts/GameMenu.cs`. Its button events and Inspector references are already connected. It uses the existing UI input setup for mouse clicks and finger taps. No extra input package is needed.

Help stays inside the current scene. A true/false value remembers whether its Back button should show the title menu or Pause. Pausing stops dialogue typing, and the menu blocks clicks from reaching scene buttons. Continue leaves an open inventory or conversation as it was.

## Scenes and build

Start_Screen is the first enabled scene in the global build list. The Android profile uses that list. A Game_Menu instance is included in all ten gameplay scenes; the older AlexTestScene remains unchanged. New gameplay scenes should include one Game_Menu instance and the usual EventSystem.

## Validation

- Unity compiled the changes and completed 184 Play mode checks with zero errors.
- Checks covered title/Help, Pause/Help, Continue, Quit, fresh-case resets, inventory/selection/conversation preservation, and button raycast blocking in all ten locations.
- Help text was visually inspected in landscape and portrait Game view previews.
- This change still needs a real Android tablet check. An existing APK must be rebuilt to include the new menus.

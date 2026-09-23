# City navigation

The ten main scenes use the existing Arrow prefab. AlexTestScene remains separate for experiments.

| Scene | Left | Up | Right | Down |
| --- | --- | --- | --- | --- |
| Office (Start) | | | Street 1 | |
| Street 1 | Office | Murder Alley | Street 2 | Dock |
| Murder Alley | | | Dumpster Alley | Street 1 |
| Dock | | Street 1 | Street 3 | |
| Dumpster Alley | Murder Alley | | | Street 2 |
| Street 2 | Street 1 | Dumpster Alley | Chief's Office | Street 3 |
| Street 3 | Dock | Street 2 | Dark Passage | Fish Market |
| Fish Market | | Street 3 | | |
| Chief's Office | Street 3 | | | |
| Dark Passage | Street 3 | | | |

The Chief's Office arrow keeps its current left-side placement and returns to Street 3, as in the supplied route list. Its label now matches the destination.

## Dark Passage replaces WIN

**WIN.unity** was renamed **Dark_Passage.unity**, preserving its asset GUID. The build scene list and Street 3 arrow use the new name. The previous two-clue WIN lock was removed: players may enter the passage early, but need to select and use the flashlight to reveal its evidence.

The game ends in **Cheifs_Office**, after presenting the final evidence with both original alley clues collected. See **CaseAdventureNotes.md** for the complete walkthrough. The culprit and exact final evidence remain story decisions for the group.

## Validation

The previous route check covered all 22 arrow routes. The September 23 case check additionally verified the updated Street 3 -> Dark Passage -> Street 3 route with no clues and the new ending at the chief. Other routes were preserved. No phone build or touch-device test has been run.

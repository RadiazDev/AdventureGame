// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using System.Collections.Generic;
using UnityEngine;

// This keeps the case stage and usable items in memory as the player moves between scenes.
public static class CaseProgress
{
    // These stages follow the story order, so a higher value means the player is farther along.
    public enum Stage
    {
        FindMurder, FindShiner, TalkDockworker, FindCrate, SellFish,
        PayShiner, GetFlashlight, SearchPassage, CollectEvidence,
        ReportChief, CaseClosed
    }

    public static Stage CurrentStage = Stage.FindMurder;
    // An empty name means the player has not selected an item to use.
    public static string SelectedItem = "";
    private static List<string> items = new List<string>();

    // This clears the case for a new game or a fresh Play session in the Editor.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetCase()
    {
        CurrentStage = Stage.FindMurder;
        SelectedItem = "";
        items.Clear();
    }

    // This checks whether the player has the named item.
    public static bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    // This adds the item only if it is not already in the inventory.
    public static void AddItem(string itemName)
    {
        if (!items.Contains(itemName)) items.Add(itemName);
    }

    // This takes an item out of the inventory.
    public static void RemoveItem(string itemName)
    {
        items.Remove(itemName);
        // Clear the selection too, so it cannot point to an item we no longer have.
        if (SelectedItem == itemName) SelectedItem = "";
    }

    // This turns the current story stage into the objective shown in the inventory.
    public static string Objective()
    {
        switch (CurrentStage)
        {
            case Stage.FindMurder: return "Investigate Murder Alley.";
            case Stage.FindShiner: return "Find the shiner in Dumpster Alley.";
            case Stage.TalkDockworker: return "Ask the dockworker how to earn a fish.";
            case Stage.FindCrate: return "Find the misplaced delivery crate at the dock.";
            case Stage.SellFish: return "Select your fish and offer it to the fish seller.";
            case Stage.PayShiner: return "Select the money and pay the shiner.";
            case Stage.GetFlashlight: return "Tell the chief about the shiner's tip.";
            case Stage.SearchPassage: return "Use the flashlight in Dark Passage, off Street 3.";
            case Stage.CollectEvidence: return "Collect the evidence revealed by the flashlight.";
            case Stage.ReportChief: return "Select the final evidence and present it to the chief.";
            default: return "Case closed. Purr-petrator caught!";
        }
    }
}

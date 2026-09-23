using System.Collections.Generic;
using UnityEngine;

// These values stay with the player when a different scene loads.
//static class to track the player's progress in the case.
public static class CaseProgress
{
    //Enum is a list of all the stages in the case. Use this to track the player's progress.
    public enum Stage
    {
        FindMurder, FindShiner, TalkDockworker, FindCrate, SellFish,
        PayShiner, GetFlashlight, SearchPassage, CollectEvidence,
        ReportChief, CaseClosed
    }

    public static Stage CurrentStage = Stage.FindMurder;
    public static string SelectedItem = "";
    private static List<string> items = new List<string>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCase()
    {
        CurrentStage = Stage.FindMurder;
        SelectedItem = "";
        items.Clear();
    }

    public static bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public static void AddItem(string itemName)
    {
        if (!items.Contains(itemName)) items.Add(itemName);
    }

    public static void RemoveItem(string itemName)
    {
        items.Remove(itemName);
        if (SelectedItem == itemName) SelectedItem = "";
    }
    //Returns the current objective based on the player's progress in the case.
    //(case stage.) This method returns a string describing the player's current objective based on the stage they are in.
    // So when the player is at a certain stage, this method will provide the corresponding objective.
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
            //Dont reorder the entries in the switch statement. Each case corresponds to a specific stage in the case.
        }
    }
}

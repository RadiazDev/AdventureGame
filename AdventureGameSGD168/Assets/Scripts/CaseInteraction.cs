using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Put this on a character or object button, then choose its role in the Inspector.
public class CaseInteraction : MonoBehaviour
{
    public enum Role { Murder, Shiner, Dockworker, DeliveryCrate, FishSeller, Chief, Darkness, HiddenEvidence, Hint }
    [SerializeField] private Role role;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private AdventureInventory bag;
    [SerializeField] private EvidenceInventory evidence;
    [SerializeField] private GameObject hiddenEvidence;
    [SerializeField] private GameObject endingPanel;
    [SerializeField, TextArea] private string hint;

    private void Start()
    {
        if (hiddenEvidence != null)
            hiddenEvidence.SetActive(CaseProgress.CurrentStage >= CaseProgress.Stage.CollectEvidence);
        if (endingPanel != null)
            endingPanel.SetActive(CaseProgress.CurrentStage == CaseProgress.Stage.CaseClosed);
    }

    public void Interact()
    {
        if (CaseProgress.CurrentStage == CaseProgress.Stage.CaseClosed) return;
        switch (role)
        {
            case Role.Murder: InspectMurder(); break;
            case Role.Shiner: TalkShiner(); break;
            case Role.Dockworker: TalkDockworker(); break;
            case Role.DeliveryCrate: FindCrate(); break;
            case Role.FishSeller: TalkFishSeller(); break;
            case Role.Chief: TalkChief(); break;
            case Role.Darkness: SearchDarkness(); break;
            case Role.HiddenEvidence: CollectFinalEvidence(); break;
            case Role.Hint: Say(hint); break;
        }
        bag.Refresh();
    }

    private void Say(string line)
    {
        dialogue.Show(new string[] { line });
    }

    private bool WrongItem(string expected)
    {
        if (CaseProgress.SelectedItem == "" || CaseProgress.SelectedItem == expected) return false;
        Say("Detective: That won't help here. I'll keep it and try something else.");
        return true;
    }

    private void InspectMurder()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.FindMurder)
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.FindShiner;
            evidence.AddEvidence("Crime scene examined");
        }
        Say("Detective: The alley cat has been murdered. Fish wrapping and a delivery receipt were left nearby. The shiner in Dumpster Alley may have seen something.");
    }

    private void TalkShiner()
    {
        if (WrongItem("Money")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.FindMurder)
        {
            Say("Shiner: Trouble in Murder Alley? Better take a look first, detective.");
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindShiner)
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.TalkDockworker;
            Say("Shiner: I saw someone leaving that alley, but information costs a coin. The fish seller buys fresh fish. Try asking the dockworker for a job.");
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.PayShiner &&
                 CaseProgress.SelectedItem == "Money" && CaseProgress.HasItem("Money"))
        {
            CaseProgress.RemoveItem("Money");
            CaseProgress.CurrentStage = CaseProgress.Stage.GetFlashlight;
            evidence.AddEvidence("Shiner testimony");
            Say("Shiner: A cat hurried out of Murder Alley and hid something in the dark passage off Street 3. Take a light. Even our eyes can't see back there.");
        }
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.GetFlashlight)
            Say("Shiner: You've already paid. Tell the chief about the passage off Street 3; you'll need a flashlight.");
        else
            Say("Shiner: One coin for my information. Select Money in your inventory, then tap me to pay.");
    }

    private void TalkDockworker()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage < CaseProgress.Stage.TalkDockworker)
            Say("Dockworker: Ask around the alleys first. Come back if you need work.");
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.TalkDockworker)
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.FindCrate;
            Say("Dockworker: Find my misplaced delivery crate and I'll pay you with a fresh fish. Look for the red crate here on the dock.");
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindCrate)
            Say("Dockworker: Tap the misplaced delivery crate. There's a fish in it for you!");
        else
            Say("Dockworker: Thanks again for finding my crate. The fish seller at the market will buy your fish.");
    }

    private void FindCrate()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage < CaseProgress.Stage.FindCrate)
            Say("Detective: A misplaced delivery crate. I should speak with the dockworker before moving it.");
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindCrate)
        {
            CaseProgress.AddItem("Fish");
            CaseProgress.CurrentStage = CaseProgress.Stage.SellFish;
            Say("Dockworker: That's my delivery crate! Here's your fish. Open your inventory, select Fish, then offer it to the fish seller.");
        }
        else
            Say("Detective: I already returned this crate and received my fish.");
    }

    private void TalkFishSeller()
    {
        if (WrongItem("Fish")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.SellFish &&
            CaseProgress.SelectedItem == "Fish" && CaseProgress.HasItem("Fish"))
        {
            CaseProgress.RemoveItem("Fish");
            CaseProgress.AddItem("Money");
            CaseProgress.CurrentStage = CaseProgress.Stage.PayShiner;
            Say("Fish seller: A fresh dock fish! Here's one coin. And that wrapping stamp? That's from this market. The receipt should help trace the victim's delivery.");
        }
        else if (CaseProgress.CurrentStage > CaseProgress.Stage.SellFish)
            Say("Fish seller: I already paid for your fish. The shiner is waiting in Dumpster Alley.");
        else
            Say("Fish seller: I'll buy a fresh fish from the dock. Select Fish in your inventory, then tap me to sell it.");
    }

    private void TalkChief()
    {
        if (WrongItem("Final evidence")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.GetFlashlight)
        {
            CaseProgress.AddItem("Flashlight");
            CaseProgress.CurrentStage = CaseProgress.Stage.SearchPassage;
            Say("Chief: The shiner saw something hidden off Street 3? Take this flashlight. Select it in your inventory and use it on the dark area. Bring any evidence back to me.");
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.ReportChief &&
                 CaseProgress.SelectedItem == "Final evidence" && CaseProgress.HasItem("Final evidence"))
        {
            if (!evidence.HasEvidence("Fish wrapping") || !evidence.HasEvidence("Delivery receipt"))
            {
                Say("Chief: Bring both original alley clues too: the fish wrapping and delivery receipt. We need the complete trail.");
                return;
            }
            CaseProgress.SelectedItem = "";
            CaseProgress.CurrentStage = CaseProgress.Stage.CaseClosed;
            endingPanel.SetActive(true);
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.ReportChief)
            Say("Chief: Select Final evidence in your inventory, then tap me to present the case.");
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.SearchPassage)
            Say("Chief: Search Dark Passage off Street 3 with the flashlight. Collect what you find, then bring it here.");
        else
            Say("Chief: Examine Murder Alley and speak with the shiner in Dumpster Alley. Bring me a useful lead.");
    }

    private void SearchDarkness()
    {
        if (WrongItem("Flashlight")) return;
        if (CaseProgress.CurrentStage >= CaseProgress.Stage.CollectEvidence)
            Say("Detective: The flashlight revealed something hidden here. I need to collect it and show the chief.");
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.SearchPassage &&
                 CaseProgress.SelectedItem == "Flashlight" && CaseProgress.HasItem("Flashlight"))
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.CollectEvidence;
            CaseProgress.SelectedItem = "";
            hiddenEvidence.SetActive(true);
            Say("Detective: There! Something was hidden in the darkness. I'll collect it before heading back to the chief.");
        }
        else if (CaseProgress.HasItem("Flashlight"))
            Say("Detective: Select the flashlight in my inventory, then tap the dark area to search it.");
        else
            Say("Detective: Too dark - even for these eyes. I'll need a flashlight. Perhaps the chief can help once I have a lead.");
    }

    private void CollectFinalEvidence()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.CollectEvidence)
        {
            CaseProgress.AddItem("Final evidence");
            CaseProgress.CurrentStage = CaseProgress.Stage.ReportChief;
            evidence.AddEvidence("Hidden evidence");
            Say("Detective: Evidence collected. Together with the alley clues and the shiner's statement, this belongs with the chief.");
        }
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.ReportChief)
            Say("Detective: I already collected this evidence. Time to take it to the chief.");
    }
}

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
    [Tooltip("Optional sound for successfully using the flashlight or collecting the hidden evidence.")]
    [SerializeField] private AudioSource successSound;
    [SerializeField, TextArea] private string[] hint;
    [SerializeField] private bool playCatCall;

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
        if (playCatCall) GameAudio.PlayCat();
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

    private void Say(string[] line)
    {
        dialogue.Show(line);
    }

    private void PlaySuccessSound()
    {
        if (successSound != null && successSound.clip != null)
            successSound.PlayOneShot(successSound.clip);
    }

    private bool WrongItem(string expected)
    {
        if (CaseProgress.SelectedItem == "" || CaseProgress.SelectedItem == expected) return false;
        Say(new string[] { "- That won't help here. I'll keep it and try something else." });
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
        Say(new string[] { "- Looks like the alley cat has been murdered.", "- Fish wrapping and a delivery receipt were left nearby. The shiner in that old Dumpster Alley may have seen something." });
    }

    private void TalkShiner()
    {
        if (WrongItem("Money")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.FindMurder)
        {
            Say(new string[] { "Shiner: Trouble in Murder Alley?", "Shiner: I ain't seen nothin'", });
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindShiner)
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.TalkDockworker;
            Say(new string[] { "Shiner: Yeah I seen somethin'. I can tell you all I know.", "Shiner: ...for a dime.", "Shiner: You ain't got one? Get a job at the docks ya bum." });
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.PayShiner &&
                 CaseProgress.SelectedItem == "Money" && CaseProgress.HasItem("Money"))
        {
            CaseProgress.RemoveItem("Money");
            CaseProgress.CurrentStage = CaseProgress.Stage.GetFlashlight;
            evidence.AddEvidence("Shiner testimony");
            Say(new string[] { "Shiner: Fine I'll tells you what I know.", "Shiner: A cat hurried out of Murder Alley and hid something in the dark passage off 3rd Street. Take a light. Even our eyes can't see back there." });
        }
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.GetFlashlight)
            Say(new string[] { "Shiner: You've already given me what I want.", "Shiner: Tell the chief about the passage off 3rd Street; you'll need a flashlight." });
        else
            Say(new string[] { "Shiner: Just a single dime for what I know.", "Shiner: Select Money in your inventory, then tap me to pay.", "Shiner: Whatever that means..." });
    }

    private void TalkDockworker()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage < CaseProgress.Stage.TalkDockworker)
            Say(new string[] { "Dockworker: Ask around the alleys first. Come back if you need work." });
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.TalkDockworker)
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.FindCrate;
            Say(new string[] { "Dockworker: Find my misplaced delivery crate and I'll pay you with a fresh fish. Look for the brown crate here on the dock.", "Dockworker: I'm colorblind so I don't see no good." });
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindCrate)
            Say(new string[] { "Dockworker: Tap the misplaced delivery crate. There's a fish in it for you!" });
        else
            Say(new string[] { "Dockworker: Thanks again for finding my crate. The fish seller at the market will buy your fish." });
    }

    private void FindCrate()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage < CaseProgress.Stage.FindCrate)
            Say(new string[] { "- A misplaced delivery crate. I should speak with the dockworker before moving it." });
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.FindCrate)
        {
            CaseProgress.AddItem("Fish");
            CaseProgress.CurrentStage = CaseProgress.Stage.SellFish;
            Say(new string[] { "Dockworker: That's my delivery crate!", "Dockworker: Wow it really was just nearby huh.", "Dockworker: Welp, here's your fish.", "Dockworker: Open your inventory, select Fish, then offer it to the fish seller.", "Dockworker: Whatever that means..." });
        }
        else
            Say(new string[] { "- I already returned this crate and received my fish." });
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
            Say(new string[] { "Fish seller: A fresh dock fish! Here's a dime kid!", "Fish Seller: Oh and that wrapping stamp? That's from this market. The receipt should help trace the victim's delivery." });
        }
        else if (CaseProgress.CurrentStage > CaseProgress.Stage.SellFish)
            Say(new string[] { "Fish seller: I already paid for your fish.", "Fish Seller: Don't you got a place to spend a dime?" });
        else
            Say(new string[] { "Fish seller: I'll buy a fresh fish from the dock. Select Fish in your inventory, then tap me to sell it.", "Fish Seller: Whatever that means..." });
    }

    private void TalkChief()
    {
        if (WrongItem("Final evidence")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.GetFlashlight)
        {
            CaseProgress.AddItem("Flashlight");
            CaseProgress.CurrentStage = CaseProgress.Stage.SearchPassage;
            Say(new string[] { "Chief: The shiner saw something hidden off 3rd street? Take this flashlight.", "Chief: Select it in your inventory and use it on the dark area. Bring any evidence back to me." });
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.ReportChief &&
                 CaseProgress.SelectedItem == "Final evidence" && CaseProgress.HasItem("Final evidence"))
        {
            if (!evidence.HasEvidence("Fish wrapping") || !evidence.HasEvidence("Delivery receipt"))
            {
                Say(new string[] { "Chief: Bring both original alley clues too: the fish wrapping and delivery receipt. We need the complete trail." });
                return;
            }
            CaseProgress.SelectedItem = "";
            CaseProgress.CurrentStage = CaseProgress.Stage.CaseClosed;
            endingPanel.SetActive(true);
            GameAudio.PlayWin();
        }
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.ReportChief)
            Say(new string[] { "Chief: Select Final evidence in your inventory, then tap me to present the case.", "Chief: Whatever that means..." });
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.SearchPassage)
            Say(new string[] { "Chief: Search the Dark Passage off 3rd street with the flashlight. Collect what you find, then bring it here." });
        else
            Say(new string[] { "Chief: Examine Murder Alley and get me a lead.", "Chief: That's an order!" });
    }

    private void SearchDarkness()
    {
        if (WrongItem("Flashlight")) return;
        if (CaseProgress.CurrentStage >= CaseProgress.Stage.CollectEvidence)
            Say(new string[] { "- The flashlight revealed something hidden here. I need to collect it and show the chief." });
        else if (CaseProgress.CurrentStage == CaseProgress.Stage.SearchPassage &&
                 CaseProgress.SelectedItem == "Flashlight" && CaseProgress.HasItem("Flashlight"))
        {
            CaseProgress.CurrentStage = CaseProgress.Stage.CollectEvidence;
            CaseProgress.SelectedItem = "";
            hiddenEvidence.SetActive(true);
            PlaySuccessSound();
            Say(new string[] { "- There! Something was hidden in the darkness. I'll collect it before heading back to the chief." });
        }
        else if (CaseProgress.HasItem("Flashlight"))
            Say(new string[] { "- Select the flashlight in my inventory, then tap the dark area to search it.", "- Whatever that means..." });
        else
            Say(new string[] { "- Too dark - even for these eyes. I'll need a flashlight.", "- Perhaps the chief can help once I have a lead." });
    }

    private void CollectFinalEvidence()
    {
        if (WrongItem("")) return;
        if (CaseProgress.CurrentStage == CaseProgress.Stage.CollectEvidence)
        {
            CaseProgress.AddItem("Final evidence");
            CaseProgress.CurrentStage = CaseProgress.Stage.ReportChief;
            evidence.AddEvidence("Hidden evidence");
            PlaySuccessSound();


            Say(new string[] { "- The murder weapon!", "- Together with the alley clues and the shiner's statement, this belongs with the chief." });
        }
        else if (CaseProgress.CurrentStage >= CaseProgress.Stage.ReportChief)
            Say(new string[] { "- I already collected this evidence. Time to take it to the chief." });
    }
}

// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This handles the item bag and lets the player pick an item to use on a target.
public class AdventureInventory : MonoBehaviour
{
    // These connect the bag's buttons and text to the objects in the Inspector.
    [SerializeField] private GameObject panel;
    [SerializeField] private EvidenceInventory evidence;
    [SerializeField] private Button fishButton;
    [SerializeField] private Button moneyButton;
    [SerializeField] private Button flashlightButton;
    [SerializeField] private Button finalEvidenceButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private TMP_Text selectionText;
    [SerializeField] private TMP_Text emptyText;
    [SerializeField] private TMP_Text objectiveText;

    // This updates the bag when its scene first starts.
    private void Start()
    {
        Refresh();
    }

    // Update the items before showing the bag so it is not showing old information.
    public void OpenInventory()
    {
        Refresh();
        panel.SetActive(true);
    }

    // Remember the chosen item and close the bag so the player can tap a target.
    public void SelectItem(string itemName)
    {
        if (!CaseProgress.HasItem(itemName)) return;
        CaseProgress.SelectedItem = itemName;
        panel.SetActive(false);
        Refresh();
    }

    // This clears the selection without taking the item out of the bag.
    public void CancelSelection()
    {
        CaseProgress.SelectedItem = "";
        Refresh();
    }

    // This shows the items we own, the selected item, and the current objective.
    public void Refresh()
    {
        // Each item button stays hidden until that item is in the bag.
        fishButton.gameObject.SetActive(CaseProgress.HasItem("Fish"));
        moneyButton.gameObject.SetActive(CaseProgress.HasItem("Money"));
        flashlightButton.gameObject.SetActive(CaseProgress.HasItem("Flashlight"));
        finalEvidenceButton.gameObject.SetActive(CaseProgress.HasItem("Final evidence"));
        // The empty message only shows when none of these four items are owned.
        emptyText.gameObject.SetActive(!CaseProgress.HasItem("Fish") &&
            !CaseProgress.HasItem("Money") && !CaseProgress.HasItem("Flashlight") &&
            !CaseProgress.HasItem("Final evidence"));

        // An empty name means nothing is selected. The ? chooses which text to show.
        bool selected = CaseProgress.SelectedItem != "";
        selectionText.text = selected ? "Using: " + CaseProgress.SelectedItem + " - tap a target" : "";
        cancelButton.SetActive(selected);
        objectiveText.text = CaseProgress.Objective();
        evidence.UpdateDisplay();
    }
}

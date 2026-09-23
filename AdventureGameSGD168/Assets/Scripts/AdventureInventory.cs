using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The item buttons select what the player will use on their next target.
public class AdventureInventory : MonoBehaviour
{
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

    private void Start()
    {
        Refresh();
    }

    public void OpenInventory()
    {
        Refresh();
        panel.SetActive(true);
    }

    public void SelectItem(string itemName)
    {
        if (!CaseProgress.HasItem(itemName)) return;
        CaseProgress.SelectedItem = itemName;
        panel.SetActive(false);
        Refresh();
    }

    public void CancelSelection()
    {
        CaseProgress.SelectedItem = "";
        Refresh();
    }

    public void Refresh()
    {
        fishButton.gameObject.SetActive(CaseProgress.HasItem("Fish"));
        moneyButton.gameObject.SetActive(CaseProgress.HasItem("Money"));
        flashlightButton.gameObject.SetActive(CaseProgress.HasItem("Flashlight"));
        finalEvidenceButton.gameObject.SetActive(CaseProgress.HasItem("Final evidence"));
        emptyText.gameObject.SetActive(!CaseProgress.HasItem("Fish") &&
            !CaseProgress.HasItem("Money") && !CaseProgress.HasItem("Flashlight") &&
            !CaseProgress.HasItem("Final evidence"));

        bool selected = CaseProgress.SelectedItem != "";
        selectionText.text = selected ? "Using: " + CaseProgress.SelectedItem + " - tap a target" : "";
        cancelButton.SetActive(selected);
        objectiveText.text = CaseProgress.Objective();
        evidence.UpdateDisplay();
    }
}

// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This lets the player click an inventory clue to read its description.
[RequireComponent(typeof(Button))]
public class EvidenceReview : MonoBehaviour
{
    [SerializeField] private EvidenceInventory inventory;
    // This name must match the clue name tracked by the inventory.
    [SerializeField] private string evidenceName;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField, TextArea] private string evidenceDescription;

    // This sets up the clue button each time it becomes active.
    private void OnEnable()
    {
        Button reviewButton = GetComponent<Button>();
        // Keep the button disabled unless the inventory confirms we have this clue.
        reviewButton.interactable = false;

        if (inventory != null)
        {
            reviewButton.interactable = inventory.HasEvidence(evidenceName);
        }

        reviewButton.onClick.AddListener(ShowDescription);
    }

    // This removes the click listener when the button becomes inactive.
    private void OnDisable()
    {
        // This keeps reopening the inventory from stacking up the same click listener.
        GetComponent<Button>().onClick.RemoveListener(ShowDescription);
    }

    // This shows the clue description if the player still has that clue.
    public void ShowDescription()
    {
        if (inventory == null || !inventory.HasEvidence(evidenceName))
        {
            return;
        }

        if (descriptionText != null)
        {
            descriptionText.text = evidenceDescription;
        }
    }
}

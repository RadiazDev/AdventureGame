//Group 1
//Ryan Diaz & Alex Freeman
//SGD 168
//Prof. Ven Lewis

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EvidencePickup : MonoBehaviour
{
    [SerializeField] private TMP_Text clueText;
    [SerializeField] private EvidenceInventory inventory;
    [SerializeField] private string evidenceName = "Fish wrapping";
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private string evidenceDescription;
    [SerializeField] private Button inventoryButton;

    private bool collected;

    private void OnEnable()
    {
        // Restore this clue when the player returns to its scene.
        if (inventory != null)
        {
            collected = inventory.HasEvidence(evidenceName);
        }

        if (collected)
        {
            GetComponent<Button>().interactable = false;

            if (clueText != null)
            {
                clueText.text = "Evidence\ncollected";
            }
        }

        if (inventoryButton != null)
        {
            inventoryButton.interactable = collected;
            inventoryButton.onClick.AddListener(ShowDescription);
        }
    }

    private void OnDisable()
    {
        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveListener(ShowDescription);
        }
    }

    public void CollectEvidence()
    {
        if (collected)
        {
            return;
        }

        if (inventory == null)
        {
            Debug.LogError("Assign the Inventory field on this clue.", this);
            return;
        }

        // Record the clue before marking this pickup as collected.
        if (!inventory.AddEvidence(evidenceName))
        {
            return;
        }

        collected = true;

        if (inventoryButton != null)
        {
            inventoryButton.interactable = true;
        }

        ShowDescription();

        if (clueText != null)
        {
            clueText.text = "Evidence\ncollected";
        }

        GetComponent<Button>().interactable = false;
        Debug.Log("Collected evidence: " + evidenceName);
    }

    // The inventory button can review a clue without collecting it again.
    public void ShowDescription()
    {
        if (collected == false)
        {
            return;
        }

        if (descriptionText != null)
        {
            descriptionText.text = evidenceDescription;
        }
    }
}

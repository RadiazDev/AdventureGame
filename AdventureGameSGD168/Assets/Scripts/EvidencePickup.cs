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

    private bool collected;

    public void CollectEvidence()
    {
        // This stops this clue from being collected again.
        if (collected)
            return;
            

            if (inventory == null)
{
    Debug.LogError("Assign the Inventory field on this clue.", this);
    return;
}

// Record the clue before marking this pickup as collected.
if (!inventory.AddEvidence(evidenceName))
    return;

        collected = true;

        if (clueText != null)
            clueText.text = "Evidence\ncollected";

        GetComponent<Button>().interactable = false;

        Debug.Log("Collected evidence: " + evidenceName);
        
    }
}

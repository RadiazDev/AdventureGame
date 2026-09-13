//Group 1
//Ryan Diaz & Alex Freeman
//SGD 168
//Prof. Ven Lewis  

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvidenceInventory : MonoBehaviour
{
    [SerializeField] private TMP_Text evidenceListText;

    private List<string> collectedEvidence = new List<string>();

    private void Start()
    {
        UpdateDisplay();
    }

    //This method adds a piece of evidence to the player's inventory if it is not already collected.
    public bool AddEvidence(string evidenceName)
    {
        if (evidenceListText == null)
        {
            Debug.LogError("Assign the Evidence List Text field.", this);
            return false;
        }

        // Only add a clue if it is not already in the players inventory.
        if (!collectedEvidence.Contains(evidenceName))
        {
            collectedEvidence.Add(evidenceName);
        }

        UpdateDisplay();
        return true;
    }

    //This method updates the display of the collected evidence in the UI.
    private void UpdateDisplay()
    {
        if (evidenceListText == null)
            return;

        evidenceListText.text = collectedEvidence.Count == 0
            ? "No evidence collected"
            : string.Join("\n", collectedEvidence);
    }
}

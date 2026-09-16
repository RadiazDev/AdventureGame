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
    [SerializeField] private TMP_Text objectiveText;

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
    {
        return;
    }

    // Show the evidence collected so far.
    if (collectedEvidence.Count == 0)
    {
        evidenceListText.text = "No evidence collected";
    }
    else
    {
        evidenceListText.text = "";

        foreach (string evidence in collectedEvidence)
        {
            evidenceListText.text += evidence + "\n";
        }
    }

    if (objectiveText == null)
    {
        return;
    }

    // Choose the objective message.
    if (collectedEvidence.Count == 0)
    {
        objectiveText.text = "Find evidence in the alley.";
    }
    else if (collectedEvidence.Count == 1)
    {
        objectiveText.text = "Find the remaining clue.";
    }
    else
    {
        objectiveText.text = "Both clues collected!";
    }
}
}

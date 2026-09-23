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

    [SerializeField] private bool useCaseObjectives;

    // Static means every scene uses the same list during this game session.
    private static List<string> collectedEvidence = new List<string>();

    // Start empty each time the game starts, including a new Unity Play session.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetEvidence()
    {
        collectedEvidence.Clear();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    public bool HasEvidence(string evidenceName)
    {
        return collectedEvidence.Contains(evidenceName);
    }

    public bool AddEvidence(string evidenceName)
    {
        if (evidenceListText == null)
        {
            Debug.LogError("Assign the Evidence List Text field.", this);
            return false;
        }

        if (!collectedEvidence.Contains(evidenceName))
        {
            collectedEvidence.Add(evidenceName);
        }

        UpdateDisplay();
        return true;
    }

    public void UpdateDisplay()
    {
        if (evidenceListText == null)
        {
            return;
        }

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

        if (useCaseObjectives)
        {
            objectiveText.text = CaseProgress.Objective();
            return;
        }

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

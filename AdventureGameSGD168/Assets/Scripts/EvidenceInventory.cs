// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This keeps the collected clues together and updates the evidence text and objective.
public class EvidenceInventory : MonoBehaviour
{
    // These are the text objects assigned in the Inspector.
    [SerializeField] private TMP_Text evidenceListText;
    [SerializeField] private TMP_Text objectiveText;

    // Turn this on for the full story; leave it off for the original two-clue test.
    [SerializeField] private bool useCaseObjectives;

    // Static keeps one shared list when scenes change. It does not save after closing the game.
    private static List<string> collectedEvidence = new List<string>();

    // This clears the clues for a new game. The attribute also resets them for a new Play session.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetEvidence()
    {
        collectedEvidence.Clear();
    }

    // Show the clues already collected when this scene starts.
    private void Start()
    {
        UpdateDisplay();
    }

    // Other scripts use this to check whether a particular clue is already collected.
    public bool HasEvidence(string evidenceName)
    {
        return collectedEvidence.Contains(evidenceName);
    }

    // Add a clue once, then refresh the display. False means it could not be recorded.
    public bool AddEvidence(string evidenceName)
    {
        // Stop here if the required text object was not assigned in the Inspector.
        if (evidenceListText == null)
        {
            Debug.LogError("Assign the Evidence List Text field.", this);
            return false;
        }

        // Checking the name first keeps the same clue from appearing twice.
        if (!collectedEvidence.Contains(evidenceName))
        {
            collectedEvidence.Add(evidenceName);
        }

        UpdateDisplay();
        return true;
    }

    // Rebuild the clue text and show the objective that matches the current progress.
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
            // Clear the old text first, then put each clue on its own line.
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

        // The full game gets its objective from the story stage instead of counting clues.
        if (useCaseObjectives)
        {
            objectiveText.text = CaseProgress.Objective();
            return;
        }

        // These simpler objectives are for the original two-clue test setup.
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

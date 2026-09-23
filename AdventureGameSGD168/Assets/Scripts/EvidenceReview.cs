//Group 1
//Ryan Diaz & Alex Freeman
//SGD 168
//Prof. Ven Lewis

using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Goes on a white inventory button, so clues can be reviewed in any scene.
[RequireComponent(typeof(Button))]
public class EvidenceReview : MonoBehaviour
{
    [SerializeField] private EvidenceInventory inventory;
    [SerializeField] private string evidenceName;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField, TextArea] private string evidenceDescription;

    private void OnEnable()
    {
        Button reviewButton = GetComponent<Button>();
        reviewButton.interactable = false;

        if (inventory != null)
        {
            reviewButton.interactable = inventory.HasEvidence(evidenceName);
        }

        reviewButton.onClick.AddListener(ShowDescription);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(ShowDescription);
    }

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

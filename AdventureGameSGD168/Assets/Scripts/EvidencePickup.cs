// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This collects a scene clue, plays its pickup sound, and unlocks its inventory button.
// These attributes make sure the clue also has a Button and an AudioSource.
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class EvidencePickup : MonoBehaviour
{
    // Each clue uses this same script with its own name, description, and references.
    [SerializeField] private TMP_Text clueText;
    [SerializeField] private EvidenceInventory inventory;
    [SerializeField] private string evidenceName = "Fish wrapping";
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private string evidenceDescription;
    [SerializeField] private Button inventoryButton;

    // This stops the scene button from collecting the same clue again.
    private bool collected;
    private AudioSource pickupSound;

    // Set up a normal 2D pickup sound that waits until the clue is clicked.
    private void Awake()
    {
        pickupSound = GetComponent<AudioSource>();
        pickupSound.playOnAwake = false;
        pickupSound.loop = false;
        pickupSound.spatialBlend = 0f;
    }

    // Check the shared evidence list and connect this clue's inventory review button.
    private void OnEnable()
    {
        // This remembers collected clues when the player returns to their scene.
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

    // Disconnect the review button when this clue object is disabled or its scene closes.
    private void OnDisable()
    {
        // Remove the listener so reopening this object does not add extra clicks.
        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveListener(ShowDescription);
        }
    }

    // Collect this clue once, play its sound, and update the scene and inventory buttons.
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

        // Only mark the clue as collected after the inventory accepts it.
        if (!inventory.AddEvidence(evidenceName))
        {
            return;
        }

        collected = true;

        // Only play after collection succeeds, not when reviewing an old clue.
        if (pickupSound.clip != null)
            pickupSound.PlayOneShot(pickupSound.clip);

        if (inventoryButton != null)
        {
            inventoryButton.interactable = true;
        }

        // Update the description text without opening the evidence panel.
        ShowDescription();

        if (clueText != null)
        {
            clueText.text = "Evidence\ncollected";
        }

        GetComponent<Button>().interactable = false;
        Debug.Log("Collected evidence: " + evidenceName);
    }

    // The inventory button uses this to review a collected clue without playing the pickup again.
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

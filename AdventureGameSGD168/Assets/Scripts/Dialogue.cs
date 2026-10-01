// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

// This handles the dialogue box, the typing effect, and moving through each line.
public class Dialogue : MonoBehaviour
{
    // These are the text box, its conversation lines, and the delay between letters.
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    // This tracks which line is showing. The first line is number 0.
    private int index;
    // Remember when the box opened so that same click does not skip the first line.
    private int openedFrame;
    // This covers the scene buttons while a conversation is open.
    [SerializeField] GameObject buttonBlocker;

    // Start at the first line each time this dialogue box opens.
    private void OnEnable()
    {
        // If the text box or lines are missing, close instead of trying to show them.
        if (textComponent == null || lines == null || lines.Length == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        openedFrame = Time.frameCount;
        index = 0;
        textComponent.text = "";
        if (buttonBlocker != null) buttonBlocker.SetActive(true);
        StartCoroutine(TypeLine());
    }

    // Stop the typing and allow scene buttons again when the box closes.
    private void OnDisable()
    {
        StopAllCoroutines();
        if (buttonBlocker != null) buttonBlocker.SetActive(false);
    }

    // Replace the conversation and reopen the box so OnEnable starts it over.
    public void Show(string[] newLines)
    {
        gameObject.SetActive(false);
        lines = newLines;
        gameObject.SetActive(true);
    }

    // Check for a mouse click or finger tap after the UI buttons have had their turn.
    private void LateUpdate()
    {
        // Ignore pause menu clicks and the click that just opened this conversation.
        if (GameMenu.BlocksDialogueInput || Time.frameCount == openedFrame)
        {
            return;
        }

        bool clicked = Mouse.current != null &&
                       Mouse.current.leftButton.wasReleasedThisFrame;

        bool tapped = Touchscreen.current != null &&
                      Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;

        if (clicked || tapped)
        {
            Advance();
        }
    }

    // Finish the current line, move to the next line, or close after the last one.
    public void Advance()
    {
        if (!gameObject.activeInHierarchy || GameMenu.BlocksDialogueInput) return;
        if (textComponent.text != lines[index])
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
        else if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = "";
            StartCoroutine(TypeLine());
        }
        else gameObject.SetActive(false);
    }

    // Show the current line one letter at a time.
    private IEnumerator TypeLine()
    {
        // This waits between letters without making the rest of the game wait.
        foreach (char letter in lines[index])
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}

// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;
using UnityEngine.UI;

// This gives a menu or inventory button the shared click sound.
[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    // Keep the Button reference so both listener methods use the same component.
    private Button button;

    // This finds the Button component before the click listener is added.
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // This connects the click sound whenever the button becomes active.
    private void OnEnable()
    {
        button.onClick.AddListener(GameAudio.PlayButton);
    }

    // This removes the listener so reopening the button will not stack up sounds.
    private void OnDisable()
    {
        button.onClick.RemoveListener(GameAudio.PlayButton);
    }
}

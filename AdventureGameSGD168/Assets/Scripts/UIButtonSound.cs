// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;
using UnityEngine.UI;

// Add to a menu or inventory button to use the shared click sound.
[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        // Subscribe while the button is active, then unsubscribe when it closes.
        button.onClick.AddListener(GameAudio.PlayButton);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameAudio.PlayButton);
    }
}

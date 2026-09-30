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
        button.onClick.AddListener(GameAudio.PlayButton);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(GameAudio.PlayButton);
    }
}

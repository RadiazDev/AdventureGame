// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;
using UnityEngine.SceneManagement;

// This connects the scene arrows and question mark buttons to their actions.
public class ButtonController : MonoBehaviour
{
    // Assign the three dialogue boxes for this scene in the Inspector.
    [SerializeField] GameObject dialogueBox1;
    [SerializeField] GameObject dialogueBox2;
    [SerializeField] GameObject dialogueBox3;

    // This covers scene buttons so the player cannot click through the dialogue.
    [SerializeField] GameObject buttonBlocker;

    // Play the footsteps and load the scene assigned to this arrow.
    public void OnArrowPress(string sceneName)
    {
        Debug.Log("Arrow Pressed to " + sceneName);
        GameAudio.PlayArrow();
        SceneManager.LoadScene(sceneName);
    }

    // Open the dialogue box number assigned to the question mark button.
    public void OnQuestionMarkPress(int dialogueBoxNumber)
    {
        Debug.Log("Question Mark Pressed");

        // Block the scene buttons until the dialogue closes.
        buttonBlocker.SetActive(true);

        switch (dialogueBoxNumber)
        {
            case 1:
                dialogueBox1.SetActive(true);
                break;
            case 2:
                dialogueBox2.SetActive(true);
                break;
            case 3:
                dialogueBox3.SetActive(true);
                break;
        }
    }
}

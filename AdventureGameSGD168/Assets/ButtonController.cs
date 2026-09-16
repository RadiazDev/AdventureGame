using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox1;
    [SerializeField] GameObject dialogueBox2;
    [SerializeField] GameObject dialogueBox3;

    [SerializeField] GameObject buttonBlocker;

    public void OnArrowPress(string sceneName)
    {
        
        Debug.Log("Arrow Pressed to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuestionMarkPress(int dialogueBoxNumber)
    {
        Debug.Log("Question Mark Pressed");

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

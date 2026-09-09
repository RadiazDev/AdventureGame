using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void OnArrowPress(string sceneName)
    {
        
        Debug.Log("Arrow Pressed to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}

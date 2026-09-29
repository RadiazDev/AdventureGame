using UnityEngine;
using UnityEngine.SceneManagement;

// The same menu prefab is used by the title screen and every game location.
public class GameMenu : MonoBehaviour
{
    [SerializeField] private bool isStartScreen;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private string startSceneName = "Start_Screen";
    [SerializeField] private string gameSceneName = "Office (Start)";

    private bool isPaused;
    private bool helpCameFromPause;
    private float previousTimeScale = 1f;
    private static int lastMenuClickFrame = -1;

    // Dialogue must also ignore the click that closes Continue or Help.
    public static bool BlocksDialogueInput
    {
        get { return Time.timeScale == 0f || Time.frameCount == lastMenuClickFrame; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetMenuInput()
    {
        lastMenuClickFrame = -1;
    }

    private void Awake()
    {
        startPanel.SetActive(isStartScreen);
        pauseButton.SetActive(!isStartScreen);
        pausePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void PauseGame()
    {
        if (isStartScreen || isPaused) return;

        lastMenuClickFrame = Time.frameCount;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        if (!isPaused) return;

        lastMenuClickFrame = Time.frameCount;
        helpPanel.SetActive(false);
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = previousTimeScale;
        isPaused = false;
    }

    public void ShowHelp()
    {
        lastMenuClickFrame = Time.frameCount;
        helpCameFromPause = isPaused;
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void BackFromHelp()
    {
        lastMenuClickFrame = Time.frameCount;
        helpPanel.SetActive(false);
        pausePanel.SetActive(helpCameFromPause);
        startPanel.SetActive(!helpCameFromPause && isStartScreen);
    }

    public void QuitToStart()
    {
        lastMenuClickFrame = Time.frameCount;
        Time.timeScale = previousTimeScale;
        isPaused = false;
        SceneManager.LoadScene(startSceneName);
    }

    public void StartGame()
    {
        lastMenuClickFrame = Time.frameCount;
        Time.timeScale = 1f;
        isPaused = false;
        CaseProgress.ResetCase();
        EvidenceInventory.ResetEvidence();
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnDisable()
    {
        // Leaving a scene or stopping Play must not leave the game frozen.
        if (isPaused)
        {
            Time.timeScale = previousTimeScale;
            isPaused = false;
        }
    }
}

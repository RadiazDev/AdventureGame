// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;
using UnityEngine.SceneManagement;

// This runs the start screen, pause menu, and Help screen using the same menu prefab.
public class GameMenu : MonoBehaviour
{
    // Set this on the start screen so it opens the title panel instead of showing Pause.
    [SerializeField] private bool isStartScreen;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private string startSceneName = "Start_Screen";
    [SerializeField] private string gameSceneName = "Office (Start)";

    private bool isPaused;
    // Remember where Help was opened so Back returns to the right menu.
    private bool helpCameFromPause;
    // Keep the old game speed so Continue can restore it after pausing.
    private float previousTimeScale = 1f;
    // Share the last menu click with dialogue so it cannot also skip a line.
    private static int lastMenuClickFrame = -1;

    // Dialogue checks this to ignore taps while paused or right after a menu button.
    public static bool BlocksDialogueInput
    {
        get { return Time.timeScale == 0f || Time.frameCount == lastMenuClickFrame; }
    }

    // Clear the old menu click whenever a new Play session starts.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetMenuInput()
    {
        lastMenuClickFrame = -1;
    }

    // Open the right panel for this scene and keep the other menus closed.
    private void Awake()
    {
        startPanel.SetActive(isStartScreen);
        pauseButton.SetActive(!isStartScreen);
        pausePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    // Pause the game and show the Continue, Quit, and Help options.
    public void PauseGame()
    {
        if (isStartScreen || isPaused) return;

        lastMenuClickFrame = Time.frameCount;
        previousTimeScale = Time.timeScale;
        // Setting game time to 0 also pauses the wait between dialogue letters.
        Time.timeScale = 0f;
        isPaused = true;
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    // Close the pause menus and return to the game at its previous speed.
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

    // Open Help and remember whether it came from the title screen or pause menu.
    public void ShowHelp()
    {
        lastMenuClickFrame = Time.frameCount;
        helpCameFromPause = isPaused;
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    // Send Back to the same menu the player used to open Help.
    public void BackFromHelp()
    {
        lastMenuClickFrame = Time.frameCount;
        helpPanel.SetActive(false);
        pausePanel.SetActive(helpCameFromPause);
        startPanel.SetActive(!helpCameFromPause && isStartScreen);
    }

    // Return to the start screen and switch back to the background audio.
    public void QuitToStart()
    {
        GameAudio.PlayBackground();
        lastMenuClickFrame = Time.frameCount;
        Time.timeScale = previousTimeScale;
        isPaused = false;
        SceneManager.LoadScene(startSceneName);
    }

    // Clear the old case and clues, then begin a new game in the office.
    public void StartGame()
    {
        GameAudio.PlayBackground();
        lastMenuClickFrame = Time.frameCount;
        Time.timeScale = 1f;
        isPaused = false;
        CaseProgress.ResetCase();
        EvidenceInventory.ResetEvidence();
        SceneManager.LoadScene(gameSceneName);
    }

    // Restore game time if this menu closes while the game is paused.
    private void OnDisable()
    {
        if (isPaused)
        {
            Time.timeScale = previousTimeScale;
            isPaused = false;
        }
    }
}

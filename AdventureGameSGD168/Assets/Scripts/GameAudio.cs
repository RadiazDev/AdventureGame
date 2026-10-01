// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;

// One copy keeps the shared sounds playing when a scene changes.
public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource arrowSource;
    [SerializeField] private AudioSource buttonSource;
    [SerializeField] private AudioSource catSource;
    [SerializeField] private AudioSource winSource;
    [SerializeField] private AudioClip[] catCalls;

    private static GameAudio instance;
    private bool winStarted;

    // Clear the shared reference when Unity starts a new Play session.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetAudio()
    {
        instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateAudio()
    {
        // Resources includes this prefab in builds and supports starting in any scene.
        if (instance != null) return;
        GameObject audioPrefab = Resources.Load<GameObject>("Game_Audio");
        if (audioPrefab != null)
            Instantiate(audioPrefab);
    }

    private void Awake()
    {
        // Keep one shared audio object and remove any duplicate from another scene.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // Normally Unity destroys scene objects when the next scene loads.
        DontDestroyOnLoad(gameObject);

        PlayBackground();
    }

    public static void PlayArrow()
    {
        if (instance == null || instance.arrowSource == null) return;
        if (instance.arrowSource.clip == null) return;

        // This source survives the scene change, so the click can finish playing.
        instance.arrowSource.PlayOneShot(instance.arrowSource.clip);
    }

    public static void PlayButton()
    {
        if (instance == null || instance.buttonSource == null) return;
        if (instance.buttonSource.clip != null)
            instance.buttonSource.PlayOneShot(instance.buttonSource.clip);
    }

    public static void PlayCat()
    {
        if (instance == null || instance.catSource == null || instance.winStarted) return;
        if (instance.catCalls == null || instance.catCalls.Length == 0) return;

        int choice = Random.Range(0, instance.catCalls.Length);
        instance.catSource.clip = instance.catCalls[choice];
        if (instance.catSource.clip != null)
            instance.catSource.Play(); // Replace the previous call instead of stacking meows.
    }

    public static void PlayWin()
    {
        if (instance == null || instance.winStarted) return;
        instance.winStarted = true;
        if (instance.backgroundSource != null) instance.backgroundSource.Stop();
        if (instance.catSource != null) instance.catSource.Stop();
        if (instance.winSource != null && instance.winSource.clip != null)
            instance.winSource.Play();
    }

    public static void PlayBackground()
    {
        if (instance == null) return;
        instance.winStarted = false;
        if (instance.winSource != null) instance.winSource.Stop();

        // Moving between normal scenes or starting a case does not restart the music.
        if (instance.backgroundSource != null && instance.backgroundSource.clip != null &&
            !instance.backgroundSource.isPlaying)
            instance.backgroundSource.Play();
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}

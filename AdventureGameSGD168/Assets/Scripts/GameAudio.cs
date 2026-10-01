// Ryan Diaz & Alex Freeman
// SGD-168
// Professor Lewis
// September 30th 2026

using UnityEngine;

// This controls the shared background, footsteps, button, cat, and win sounds.
// I keep one audio object between scenes so changing locations does not cut these sounds off.
public class GameAudio : MonoBehaviour
{
    // The Audio Sources and cat clips are assigned on the Game_Audio prefab in Resources.
    // Separate sources let a button or cat sound play while the background keeps going.
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource arrowSource;
    [SerializeField] private AudioSource buttonSource;
    [SerializeField] private AudioSource catSource;
    [SerializeField] private AudioSource winSource;
    [SerializeField] private AudioClip[] catCalls;

    private static GameAudio instance;
    // This stops the ending sound from starting again if another call happens after winning.
    private bool winStarted;

    // Clear the shared reference when Unity starts a new Play session.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetAudio()
    {
        instance = null;
    }

    // Make the shared audio object before the first scene loads, even when testing a later scene.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateAudio()
    {
        // Resources includes this prefab in builds and supports starting in any scene.
        if (instance != null) return;
        GameObject audioPrefab = Resources.Load<GameObject>("Game_Audio");
        if (audioPrefab != null)
            Instantiate(audioPrefab);
    }

    // Keep the first audio object, remove extra copies, and start the background sound.
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

    // Navigation buttons call this to play footsteps before loading the next location.
    // Static lets them call GameAudio.PlayArrow() without finding the audio object in each scene.
    public static void PlayArrow()
    {
        if (instance == null || instance.arrowSource == null) return;
        if (instance.arrowSource.clip == null) return;

        // This source survives the scene change, so the footsteps can finish playing.
        instance.arrowSource.PlayOneShot(instance.arrowSource.clip);
    }

    // Play the shared UI click sound. Missing sources or clips are skipped safely.
    public static void PlayButton()
    {
        if (instance == null || instance.buttonSource == null) return;
        if (instance.buttonSource.clip != null)
            instance.buttonSource.PlayOneShot(instance.buttonSource.clip);
    }

    // Pick one cat call at random when a character interaction asks for it.
    // Repeated clicks replace the last call, and cats stay quiet after the case is closed.
    public static void PlayCat()
    {
        if (instance == null || instance.catSource == null || instance.winStarted) return;
        if (instance.catCalls == null || instance.catCalls.Length == 0) return;

        int choice = Random.Range(0, instance.catCalls.Length);
        instance.catSource.clip = instance.catCalls[choice];
        if (instance.catSource.clip != null)
            instance.catSource.Play(); // Replace the previous call instead of stacking meows.
    }

    // Stop the background and cat sounds, then play the ending sound once.
    public static void PlayWin()
    {
        if (instance == null || instance.winStarted) return;
        instance.winStarted = true;
        if (instance.backgroundSource != null) instance.backgroundSource.Stop();
        if (instance.catSource != null) instance.catSource.Stop();
        if (instance.winSource != null && instance.winSource.clip != null)
            instance.winSource.Play();
    }

    // Return to normal background audio when a case starts or we return to the title screen.
    // If it is already playing, leave it alone instead of starting the track over.
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

    // Clear the shared reference only if the audio object we were using is being destroyed.
    // Removing an extra copy should not disconnect the one that is still playing.
    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}

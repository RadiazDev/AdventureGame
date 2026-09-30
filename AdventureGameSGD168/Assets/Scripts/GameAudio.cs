using UnityEngine;

// One copy keeps music and arrow sounds playing when a scene changes.
public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioSource arrowSource;

    private static GameAudio instance;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // An empty clip slot is fine while the team is choosing sounds.
        if (backgroundSource != null && backgroundSource.clip != null)
            backgroundSource.Play();
    }

    public static void PlayArrow()
    {
        if (instance == null || instance.arrowSource == null) return;
        if (instance.arrowSource.clip == null) return;

        // This source survives the scene change, so the click can finish playing.
        instance.arrowSource.PlayOneShot(instance.arrowSource.clip);
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}

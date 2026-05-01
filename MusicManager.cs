using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip[] levelMusic; // different track per level

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu"||scene.name=="LEVELS")
        {
            PlayMusic(menuMusic);
        }
        else if (scene.name.StartsWith("Level"))
        {
            int levelIndex = ExtractLevelIndex(scene.name);
            PlayMusic(levelMusic[levelIndex]);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    int ExtractLevelIndex(string sceneName)
    {
        // Example: "Level1" → 0, "Level2" → 1
        string number = sceneName.Replace("Level", "");
        return int.Parse(number) - 1;
    }
}
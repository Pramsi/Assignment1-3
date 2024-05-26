using UnityEngine;
using AK.Wwise;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AK.Wwise.Bank firstSceneBank;
    public AK.Wwise.Bank secondSceneBank;

    private string currentSceneName;

    private uint backgroundId;
    private uint restaurantAmbienceId;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AkSoundEngine.SetState("background", "backgroundNoise");
        backgroundId = AkSoundEngine.PostEvent("Play_background", gameObject);
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadBankForScene(currentSceneName);
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        UnloadBankForScene(currentSceneName);
        currentSceneName = scene.name;
        LoadBankForScene(currentSceneName);
        Debug.LogWarning(currentSceneName);


        if (currentSceneName == "FirstScene")
        {
            Debug.LogWarning("Background");
            AkSoundEngine.SetState("background", "backgroundNoise");
            backgroundId = AkSoundEngine.PostEvent("Play_background", gameObject);
            AkSoundEngine.StopPlayingID(restaurantAmbienceId);

            AkSoundEngine.SetSwitch("walking", "street", gameObject);
        }
        else if (currentSceneName == "Indoor")
        {
            AkSoundEngine.StopPlayingID(backgroundId);

            Debug.LogWarning("HHHELELEOFIEHJFOIE");
            AkSoundEngine.SetSwitch("walking", "wood", gameObject);

            AkSoundEngine.PostEvent("Play_Bell", gameObject);
            restaurantAmbienceId = AkSoundEngine.PostEvent("Play_RestaurantAmbience", gameObject);
        }
    }

    private void LoadBankForScene(string sceneName)
    {
        if (sceneName == "FirstScene")
        {
            firstSceneBank.Load();
        }
        else if (sceneName == "Indoor")
        {
            secondSceneBank.Load();
        }
    }

    private void UnloadBankForScene(string sceneName)
    {
        if (sceneName == "FirstScene")
        {
            firstSceneBank.Unload();
        }
        else if (sceneName == "Indoor")
        {
            secondSceneBank.Unload();
        }
    }
}

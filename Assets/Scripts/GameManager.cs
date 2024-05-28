using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerStatistics savedPlayerData = new PlayerStatistics();
    public GameObject playerPrefab;

    [HideInInspector]
    public GameObject playerInstance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            SpawnPlayer();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerInstance);
        }
    }

    public void MovePlayerToSpawnPoint(Vector3 spawnPosition)
    {
        if (playerInstance != null)
        {
            playerInstance.transform.position = spawnPosition;

            foreach (Transform child in playerInstance.transform)
            {
                child.localPosition = Vector3.zero;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

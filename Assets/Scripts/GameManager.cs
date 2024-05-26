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
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            Debug.Log("Spawning player from GameManager.");
            playerInstance = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerInstance);
        }
        else
        {
            Debug.LogWarning("PlayerPrefab is not assigned in the GameManager.");
        }
    }

    public void MovePlayerToSpawnPoint(Vector3 spawnPosition)
    {
        if (playerInstance != null)
        {
            Debug.Log("Moving player to spawn point: " + spawnPosition);
            playerInstance.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogWarning("PlayerInstance is not available.");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

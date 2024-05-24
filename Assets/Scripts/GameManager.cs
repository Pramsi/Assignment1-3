using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerStatistics savedPlayerData = new PlayerStatistics();
    public GameObject playerPrefab;

    [HideInInspector]
    public GameObject playerInstance;

    void Awake()
    {
        Application.targetFrameRate = 60;
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
        playerInstance = Instantiate(playerPrefab);
        DontDestroyOnLoad(playerInstance);
    }

    public void MovePlayerToSpawnPoint(Vector3 spawnPosition)
    {
        if (playerInstance != null)
        {
            playerInstance.transform.position = spawnPosition;
        }
    }
}

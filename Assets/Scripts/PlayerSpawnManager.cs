using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(MovePlayerToSpawnPoint());
    }

    private IEnumerator MovePlayerToSpawnPoint()
    {
        yield return null; // Wait one frame to ensure GameManager and playerInstance are ready
        if (GameManager.Instance != null && GameManager.Instance.playerInstance != null)
        {
            GameManager.Instance.MovePlayerToSpawnPoint(spawnPoint.position);
        }
    }
}

using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager instance is null. Instantiating GameManager.");
            GameObject gameManager = Instantiate(gameManagerPrefab);
            DontDestroyOnLoad(gameManager);
        }
        else
        {
            Debug.Log("GameManager instance already exists.");
        }
    }
}

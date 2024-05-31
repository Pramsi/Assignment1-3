using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameObject gameManager = Instantiate(gameManagerPrefab);
            DontDestroyOnLoad(gameManager);
        }
    }
}

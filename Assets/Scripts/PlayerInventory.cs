using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;


    public PlayerStatistics localPlayerData = new PlayerStatistics();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return; // Early return to avoid running Start on the destroyed instance
        }
    }
    void Start()
    {
        if (GameManager.Instance != null)
        {
            localPlayerData = GameManager.Instance.savedPlayerData;
        }
    }

    public void SavePlayerData()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.savedPlayerData = localPlayerData;
        }
    }

}

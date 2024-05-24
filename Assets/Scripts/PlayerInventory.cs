using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;


    public PlayerStatistics localPlayerData = new PlayerStatistics();




    public int collectedCoins { get; private set; }
    public int health { get; private set; } = 5;


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        GameManager.Instance.Player = gameObject;
    }

    //At start, load data from GlobalControl.
    void Start()
    {
        localPlayerData = GameManager.Instance.savedPlayerData;
    }

}

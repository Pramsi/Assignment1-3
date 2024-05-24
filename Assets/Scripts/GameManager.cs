using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerStatistics savedPlayerData = new PlayerStatistics();


    public GameObject Player;





    // Reference to the player's inventory
    //private PlayerInventory playerInventory;


    private bool _gameWon = false;
    private int _winMessage = 0;

    public int collectedCoins;
    public int health;

    void Awake()
    {
        Application.targetFrameRate = 144;

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
    // Start is called before the first frame update
    //void Start()
    //{
    //    //if (Instance != null)
    //    //    return;
    //    //Instance = this;
    //    playerInventory = GetComponent<PlayerInventory>();
    //    AkSoundEngine.SetRTPCValue("health", playerInventory.health);  // Set initial RTPC value
    //    AkSoundEngine.PostEvent("Play_healthRepresentation", this.gameObject);  // Post event to start heartbeat sound
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if(playerInventory.collectedCoins == 9)
    //    {
    //        AkSoundEngine.SetState("background", "winningMusic");
    //        if (_winMessage < 1)
    //        {
    //            AkSoundEngine.PostEvent("Play_background", gameObject);

    //            Debug.ClearDeveloperConsole();
    //        Debug.Log("You Win");
    //        _gameWon = true;
    //        }
            
    //        _winMessage++;
    //    }

    //    if (!_gameWon)
    //    {
        
    //    //Debug.Log("Collected Coins: " + collectedCoins);
    //    //Debug.Log("Current Health: " + health);
    //    }

    //}    //void Update()
    //{
    //    if(playerInventory.collectedCoins == 9)
    //    {
    //        AkSoundEngine.SetState("background", "winningMusic");
    //        if (_winMessage < 1)
    //        {
    //            AkSoundEngine.PostEvent("Play_background", gameObject);

    //            Debug.ClearDeveloperConsole();
    //        Debug.Log("You Win");
    //        _gameWon = true;
    //        }
            
    //        _winMessage++;
    //    }

    //    if (!_gameWon)
    //    {
        
    //    //Debug.Log("Collected Coins: " + collectedCoins);
    //    //Debug.Log("Current Health: " + health);
    //    }

    //}
}

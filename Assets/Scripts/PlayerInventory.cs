using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    private bool _initialLifesSet = false;
    public static PlayerInventory Instance;

    public PlayerStatistics localPlayerData = new PlayerStatistics();

    private bool _winningSoundPlayed = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return; // Early return to avoid running Start on the destroyed instance
        }

        if (!_initialLifesSet)
        {
            Instance.localPlayerData.health += 5;
            Instance.SavePlayerData();
            _initialLifesSet = true;
        }
    }


    private void Update()
    {
        if (!_winningSoundPlayed) { 
            if (Instance.localPlayerData.coins == 9)
            {
            AkSoundEngine.SetState("background", "winningMusic");
            AkSoundEngine.PostEvent("Play_background", gameObject);
            _winningSoundPlayed=true;
            }
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

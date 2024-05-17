using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    public static GameManager instance;


    [SerializeField] private int health;
    [SerializeField] private int collectedCoins;

    private bool _gameWon = false;
    private int _winMessage = 0;

    public int GetHealth() => health;

    public void DecreaseHealth(int decreaseBy) => health -= decreaseBy;
    public void IncreaseHealth(int increaseBy) => health += increaseBy;

    public int GetCoins() => collectedCoins;

    public void DecreaseCoins(int decreaseBy) => collectedCoins -= decreaseBy;
    public void IncreaseCoins(int increaseBy) => collectedCoins += increaseBy;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            return;
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(collectedCoins == 9)
        {
            if (_winMessage < 1)
            {
             Debug.ClearDeveloperConsole();
            Debug.Log("You Win");
            _gameWon = true;   
            }
            
            _winMessage++;
        }

        if (!_gameWon)
        {
        Debug.Log("Collected Coins: " + collectedCoins);
        Debug.Log("Current Health: " + health);
        }

    }
}

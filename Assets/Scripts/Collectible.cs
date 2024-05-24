using UnityEngine;
using System.Collections;


public class Collectible : MonoBehaviour
{
    public int coinAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            PlayerInventory.Instance.localPlayerData.coins += coinAmount;
                
                gameObject.SetActive(false);
            
        }
    }
}

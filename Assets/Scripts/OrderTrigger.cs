using System.Collections;
using UnityEngine;

public class OrderTrigger : MonoBehaviour
{
    public GameObject dialogWindowPrefab;
    public int coffeeCost = 5;
    public int healthToAdd = 10;

    private bool dialogActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogActive)
        {
            // Instantiate the dialog window prefab
            GameObject dialogWindow = Instantiate(dialogWindowPrefab);
            dialogWindow.SetActive(true); // Make sure it's active
            dialogActive = true;

            // Listen for player input
            StartCoroutine(HandleInput(dialogWindow));
        }
    }

    private IEnumerator HandleInput(GameObject dialogWindow)
    {
        while (dialogActive)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                // Player wants to buy coffee
                if (PlayerInventory.Instance.localPlayerData.coins >= coffeeCost)
                {
                    // Deduct coins and add health
                    PlayerInventory.Instance.localPlayerData.coins -= coffeeCost;
                    PlayerInventory.Instance.localPlayerData.health = 5;

                    Debug.Log("Coffee bought! Health added.");

                    // Close the dialog window
                    Destroy(dialogWindow);
                    dialogActive = false;
                }
                else
                {
                    Debug.Log("Not enough coins to buy coffee.");
                    // Display a message indicating insufficient coins
                }
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                // Player cancels the purchase
                Debug.Log("Coffee purchase canceled.");
                // Close the dialog window
                Destroy(dialogWindow);
                dialogActive = false;
            }

            yield return null;
        }
    }
}

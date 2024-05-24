using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthDecrease = 1;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            PlayerInventory.Instance.localPlayerData.health -= healthDecrease;
        }
    }
}

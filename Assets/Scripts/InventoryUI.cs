using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI HealthText;

    private void Start()
    {

    }

    private void Update()
    {
        CoinText.text = PlayerInventory.Instance.localPlayerData.coins.ToString();
        HealthText.text = PlayerInventory.Instance.localPlayerData.health.ToString();
    }
}

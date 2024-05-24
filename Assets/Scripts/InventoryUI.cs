using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI HealthText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (PlayerInventory.Instance != null)
        {
            CoinText.text = PlayerInventory.Instance.localPlayerData.coins.ToString();
            HealthText.text = PlayerInventory.Instance.localPlayerData.health.ToString();
        }
    }
}

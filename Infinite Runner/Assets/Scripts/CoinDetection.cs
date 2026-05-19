using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Count")]
    public int coinCount = 0;

    [Header("UI References")]
    public TMP_Text coinText;
    public TMP_Text normalText;

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;

            UpdateUI();

            // Instead of Destroy, disable the coin
            other.gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = " : " + coinCount;
        }

        if (normalText != null)
        {
            normalText.text = "Coins: " + coinCount;
        }
    }
}
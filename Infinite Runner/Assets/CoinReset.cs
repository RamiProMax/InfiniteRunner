using UnityEngine;

public class ReactivateCoins : MonoBehaviour
{
    private void OnEnable()
    {
        // Reactivate all coins inside this segment
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Coin"))
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
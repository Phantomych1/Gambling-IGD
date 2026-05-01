using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private int _rewardAmount = 50;

    /// <summary>
    /// Adds the reward amount to the player's balance when the player collides with the coin, and then destroys the coin object.<br/>
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BalanceManager.Instance.RemoveMoneyFromPlayer(_rewardAmount);

            Destroy(gameObject);
        }
    }
}
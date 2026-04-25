using System;
using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    public static BalanceManager Instance { get; private set; }
    public int Playerbalance { get; private set;}

    public static event Action<int> OnBalanceChanged;


    /// <summary>
    /// Adds specific amount of money to player balance. If the amount is negative, it will not be added.<br/>
    /// Triggers the OnBalanceChanged event after updating the balance, passing the new balance as a parameter.<br/>
    /// </summary>
    /// <param name="amount">The amount of money to add to the player's balance.</param>
    public void AddMoneyToPlayer(int amount)
    {
        if (amount < 0) return;
        Playerbalance += amount;
        Debug.Log($"Player Balance: {Playerbalance}");

        OnBalanceChanged?.Invoke(Playerbalance);
    }

    /// <summary>
    /// Takes specific amount of money from player balance. If the amount is negative, it will not be deducted.<br/>
    /// Triggers the OnBalanceChanged event after updating the balance, passing the new balance as a parameter.<br/>
    /// </summary>
    /// <param name="amount">The amount of money to add to the player's balance.</param>
    public void RemoveMoney(int amount)
    {
        if (amount < 0) return;
        if (Playerbalance >= amount)
        {
            Playerbalance -= amount;
        }

        OnBalanceChanged?.Invoke(Playerbalance);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Playerbalance = 1000;
        OnBalanceChanged?.Invoke(Playerbalance);
    }
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SlotSymbol
{
    public string symbolName;
    public int dropWeight;
}

public class OneHandedBanditManager : MonoBehaviour
{
    public List<SlotSymbol> symbols;
    public int _rewardAmount = 1000;
    public string[] lastSpinResult;

    public string GetRandomSymbol()
    {
        int totalWeight = 0;

        foreach (var symbol in symbols)
        {
            totalWeight += symbol.dropWeight;
        }

        int randomValue = Random.Range(0, totalWeight);

        foreach (var symbol in symbols)
        {
            if (randomValue < symbol.dropWeight)
            {
                return symbol.symbolName;
            }
            randomValue -= symbol.dropWeight;
        }

        return symbols[0].symbolName;
    }

    public string[] Spin()
    {
        string[] result = new string[3];
        result[0] = GetRandomSymbol();
        result[1] = GetRandomSymbol();
        result[2] = GetRandomSymbol();

        lastSpinResult = result;

        return result;
    }

    public void PlayMachine()
    {
        BalanceManager.Instance.RemoveMoneyFromPlayer(100);

        if (BalanceManager.Instance.Playerbalance >= 100)
        {
            string[] spinResult = Spin();

            Debug.Log($"На барабанах: {spinResult[0]} | {spinResult[1]} | {spinResult[2]}");

            if (spinResult[0] == spinResult[1] && spinResult[1] == spinResult[2])
            {
                Debug.Log($"Победа! Вы собрали комбинацию из трех: {spinResult[0]}");
                BalanceManager.Instance.AddMoneyToPlayer(_rewardAmount);
            }
            else
            {
                Debug.Log("Ничего не совпало. Попробуйте еще раз!");
            }
        }
        else
        {
            Debug.Log("Недостаточно средств для игры. Пожалуйста, пополните баланс.");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(GetRandomSymbol());
        }
    }
}
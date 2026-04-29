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

    /// <summary>
    /// Selects a symbol name at random from the available symbols, using their associated drop weights to determine the
    /// probability of selection.
    /// </summary>
    /// <remarks>If the total drop weight is zero or an unexpected condition occurs, the method returns the
    /// name of the first symbol in the collection.</remarks>
    /// <returns>A string containing the name of the randomly selected symbol. The probability of each symbol being selected is
    /// proportional to its drop weight.</returns>
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

    // Метод для самого взаимодействия (вызывается, когда игрок жмет 'E' и запускает игру)
    public void PlayMachine()
    {
        BalanceManager.Instance.AddMoneyToPlayer(_rewardAmount);

        // 2. Получаем результаты спина
        string[] spinResult = Spin();

        Debug.Log($"На барабанах: {spinResult[0]} | {spinResult[1]} | {spinResult[2]}");

        // 3. Проверяем комбинацию (совпали ли все три символа)
        if (spinResult[0] == spinResult[1] && spinResult[1] == spinResult[2])
        {
            Debug.Log($"Победа! Вы собрали комбинацию из трех: {spinResult[0]}");

            // Здесь можно добавить логику выдачи приза в зависимости от того, какой именно символ выпал.
            // Например, три семерки дают больше денег, чем три вишенки.
        }
        else
        {
            Debug.Log("Ничего не совпало. Попробуйте еще раз!");
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

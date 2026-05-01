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

    // Переменная для награды
    public int _rewardAmount = 100;

    // Та самая переменная-память для связи с UI барабанами
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

    // Метод генерации спина
    public string[] Spin()
    {
        string[] result = new string[3];
        result[0] = GetRandomSymbol();
        result[1] = GetRandomSymbol();
        result[2] = GetRandomSymbol();

        // Сохраняем результат, чтобы UI мог его прочитать
        lastSpinResult = result;

        return result;
    }

    // Метод для самого взаимодействия 
    public void PlayMachine()
    {
        // Пока временно закомментировано, чтобы Unity не ругалась на отсутствие BalanceManager
        // BalanceManager.Instance.AddMoneyToPlayer(_rewardAmount);

        // Получаем результаты спина
        string[] spinResult = Spin();

        Debug.Log($"На барабанах: {spinResult[0]} | {spinResult[1]} | {spinResult[2]}");

        // Проверяем комбинацию (совпали ли все три символа)
        if (spinResult[0] == spinResult[1] && spinResult[1] == spinResult[2])
        {
            Debug.Log($"Победа! Вы собрали комбинацию из трех: {spinResult[0]}");
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
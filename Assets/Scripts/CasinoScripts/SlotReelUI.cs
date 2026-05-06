using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotReelUI : MonoBehaviour
{
    [Header("Настройки UI")]
    public Image reelImage;
    public Sprite[] allSymbols;

    [Header("Настройки прокрута")]
    public float spinDuration = 2f;
    public float spinSpeed = 0.05f;

    // Этот метод запускает анимацию прокрута
    public void StartSpin(string finalSymbolName)
    {
        StartCoroutine(SpinRoutine(finalSymbolName));
    }

    private IEnumerator SpinRoutine(string finalSymbolName)
    {
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            int randomIndex = Random.Range(0, allSymbols.Length);
            reelImage.sprite = allSymbols[randomIndex];
            
            yield return new WaitForSeconds(spinSpeed);
            elapsedTime += spinSpeed;
        }

        Sprite finalSprite = GetSpriteByName(finalSymbolName);
        if (finalSprite != null)
        {
            reelImage.sprite = finalSprite;
        }
        else
        {
            Debug.LogError($"Спрайт с именем '{finalSymbolName}' не найден! Проверь имена нарезанных спрайтов.");
        }
    }

    private Sprite GetSpriteByName(string name)
    {
        foreach (var sprite in allSymbols)
        {
            if (sprite.name == name) return sprite;
        }
        return null;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotReelUI : MonoBehaviour
{
    [Header("UI")]
    public Image reelImage;
    public Sprite[] allSymbols;

    [Header("Spin")]
    public float spinDuration = 2f;
    public float spinSpeed = 0.05f;

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
            Debug.LogError($"Sprite '{finalSymbolName}' not found!");
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
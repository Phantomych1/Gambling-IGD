using UnityEngine;
using UnityEngine.Events; // Добавили библиотеку событий
using UnityEngine.UI;
using System.Collections;

public class SlotMachineInteractor : MonoBehaviour
{
    [Header("Настройки UI")]
    public GameObject slotMachineMenuUI;
    public SlotReelUI[] reels;

    [Header("Настройки игры")]
    public int costPerSpin = 10;

    [Header("События (Что делать с игроком)")]
    public UnityEvent onMenuOpen;  // Вызовется, когда меню откроется
    public UnityEvent onMenuClose; // Вызовется, когда меню закроется

    private bool isPlayerNear = false;
    private OneHandedBanditManager banditManager;
    private bool isSpinning = false;

    void Start()
    {
        banditManager = GetComponent<OneHandedBanditManager>();
        if (slotMachineMenuUI != null) slotMachineMenuUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isSpinning)
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (slotMachineMenuUI == null) return;

        bool isActive = !slotMachineMenuUI.activeSelf;
        slotMachineMenuUI.SetActive(isActive);

        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;

        // Вместо кода вызываем настройки из Инспектора
        if (isActive) onMenuOpen.Invoke();
        else onMenuClose.Invoke();
    }

    public void InsertCoinAndPlay()
    {
        if (isSpinning) return;

        Debug.Log($"Монеты внесены (Списано {costPerSpin}).");

        if (banditManager != null)
        {
            banditManager.PlayMachine();
            StartCoroutine(AnimateReelsRoutine(banditManager.lastSpinResult));
        }
    }

    private IEnumerator AnimateReelsRoutine(string[] results)
    {
        isSpinning = true;

        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] != null && results != null && i < results.Length)
            {
                reels[i].StartSpin(results[i]);
            }
        }

        if (reels.Length > 0 && reels[0] != null)
        {
            yield return new WaitForSeconds(reels[0].spinDuration);
        }

        isSpinning = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (slotMachineMenuUI != null) slotMachineMenuUI.SetActive(false);
            onMenuClose.Invoke(); // Включаем игрока обратно, если он отошел
        }
    }
}
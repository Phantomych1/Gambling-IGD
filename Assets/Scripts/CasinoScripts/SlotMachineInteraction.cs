using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class SlotMachineInteraction : MonoBehaviour
{
    [Header("Настройки UI")]
    public GameObject slotMachineMenuUI;
    public SlotReelUI[] reels;

    [Header("Настройки игры")]
    public int costPerSpin = 100;

    [Header("События")]
    public UnityEvent onMenuOpen;
    public UnityEvent onMenuClose;

    private bool isPlayerNear = false;
    private OneHandedBanditManager banditManager;
    private bool isSpinning = false;

    void Start()
    {
        banditManager = GetComponent<OneHandedBanditManager>();
        
        // Dynamically find UI if not assigned (useful for prefabs)
        if (slotMachineMenuUI == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform uiTransform = canvas.transform.Find("SlotMachineUI");
                if (uiTransform != null)
                {
                    slotMachineMenuUI = uiTransform.gameObject;
                    // If we found the UI, also try to find the reels inside it
                    Transform reelsTransform = uiTransform.Find("Reels");
                    if (reelsTransform != null && (reels == null || reels.Length == 0))
                    {
                        reels = reelsTransform.GetComponentsInChildren<SlotReelUI>();
                    }
                }
            }
        }

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

        // If opening the menu, wire the button to THIS machine
        if (isActive)
        {
            Button playButton = slotMachineMenuUI.GetComponentInChildren<Button>();
            if (playButton != null)
            {
                playButton.onClick.RemoveAllListeners();
                playButton.onClick.AddListener(InsertCoinAndPlay);
            }
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var move = player.GetComponent<PlayerMovement>();
            var cam = player.GetComponent<CameraMovement>();
            if (move != null) move.enabled = !isActive;
            if (cam != null) cam.enabled = !isActive;
        }

        if (isActive) onMenuOpen.Invoke();
        else onMenuClose.Invoke();
    }

    public void InsertCoinAndPlay()
    {
        if (isSpinning) return;

        if (banditManager != null)
        {
            banditManager.PlayMachine();
            if (banditManager.lastSpinResult != null && banditManager.lastSpinResult.Length > 0)
            {
                StartCoroutine(AnimateReelsRoutine(banditManager.lastSpinResult));
            }
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
            yield return new WaitForSeconds(reels[0].spinDuration + 0.5f);
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
            if (slotMachineMenuUI != null && slotMachineMenuUI.activeSelf)
            {
                ToggleMenu();
            }
        }
    }
}
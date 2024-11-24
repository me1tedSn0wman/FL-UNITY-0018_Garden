using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private WindowUI settingsUI;
    [SerializeField] private InfoUI infoUI;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private GameplayUpgradesUI gameplayUpgradesUI;
    

    [Header("Buttons")]
    [SerializeField] private Button button_Pause;
    [SerializeField] private Button button_ShowUpgrades;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI text_timerValue;
    [SerializeField] private TextMeshProUGUI text_MoneyValue;
    [SerializeField] private TextMeshProUGUI text_HealthValue;
    [SerializeField] private TextMeshProUGUI text_CapacityValue;
    [SerializeField] private TextMeshProUGUI text_DiamondsValue;

    public event Action OnPauseClicked;
    public event Action OnResumeClicked;

    // Start is called before the first frame update
    public void Start()
    {
        button_Pause.onClick.AddListener(() =>
        {
            pauseUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
            OnPauseClicked();
        });

        button_ShowUpgrades.onClick.AddListener(() =>
        {
            gameplayUpgradesUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        pauseUI.gameplayUIManager = this;

        pauseUI.SetActive(false);
        settingsUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameplayUpgradesUI.SetActive(false);

        Subscribe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimerValue(float time)
    {
        int time_sec = Mathf.FloorToInt(time);
        text_timerValue.text = string.Format("{0:d3}", time_sec);
    }

    public void UpdateMoneyValue(int money)
    {
        text_MoneyValue.text = string.Format("{0:d5}", money);
    }

    public void UpdateHealthValue(int health)
    {
        text_HealthValue.text = string.Format("{0:d1}", health);
    }

    public void UpdateCapacityValue(int crntValue, int maxCapacity) {
        text_CapacityValue.text = string.Format("{0:d2}/{1:d2}", crntValue, maxCapacity);
    }

    public void UpdateDiamondsValue(int diamonds) {
        text_DiamondsValue.text = string.Format("{0:d5}", diamonds);
    }

    public void ShowSettingsUI() {
        settingsUI.SetActive(true);
    }

    public void ShowInfoUI()
    {
        infoUI.SetActive(true);
    }

    public void ToMainMenu() {
        GameManager.LOAD_MAIN_MENU_SCENE();
    }

    public void ResumeGame() {
        pauseUI.SetActive(false);
        OnResumeClicked();
    }

    public void GameOver() {
        gameOverUI.SetActive(true);
    }

    public void SpawnUpgradableIcons(GameplayUpgrade[] upgrades) {
        gameplayUpgradesUI.SpawnUpgradableIcons(upgrades);
    }

    public void Subscribe() {
        GameManager.Instance.OnDiamondsValueChange += UpdateDiamondsValue;
    }

    public void Unsubscribe() {
        GameManager.Instance.OnDiamondsValueChange -= UpdateDiamondsValue;
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }
}

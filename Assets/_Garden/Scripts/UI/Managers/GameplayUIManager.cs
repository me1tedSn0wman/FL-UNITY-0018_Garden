using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private WindowUI settingsUI;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private WindowUI gameplayUpgradesUI;

    [Header("Buttons")]
    [SerializeField] private Button button_Pause;
    [SerializeField] private Button button_ShowUpgrades;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI text_timerValue;
    [SerializeField] private TextMeshProUGUI text_MoneyValue;
    [SerializeField] private TextMeshProUGUI text_HealthValue;

    public event Action OnPauseClicked;
    public event Action OnResumeClicked;

    // Start is called before the first frame update
    public void Start()
    {
        button_Pause.onClick.AddListener(() =>
        {
            pauseUI.SetActive(true);
            OnPauseClicked();
        });

        button_ShowUpgrades.onClick.AddListener(() =>
        {
            gameplayUpgradesUI.SetActive(true);
        });

        pauseUI.gameplayUIManager = this;

        pauseUI.SetActive(false);
        settingsUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameplayUpgradesUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimerValue(float time)
    {
        int time_sec = Mathf.FloorToInt(time) % 60;
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

    public void ShowSettingsUI() {
        settingsUI.SetActive(true);
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
}

using UnityEngine;
using UnityEngine.UI;

public class PauseUI : WindowUI
{

    [Header("Buttons")]
    [SerializeField] private Button button_ResumeGame;
    [SerializeField] private Button button_Settings;
    [SerializeField] private Button button_ToMainMenu;

    [Header("Set Dynamically")]
    public GameplayUIManager gameplayUIManager;

    public override void Awake()
    {

    }

    public void Start()
    {
        button_CloseWindowCanvas.onClick.AddListener(() =>
        {
            gameplayUIManager.ResumeGame();
        });

        button_ResumeGame.onClick.AddListener(() =>
        {
            gameplayUIManager.ResumeGame();
        });

        button_Settings.onClick.AddListener(() =>
        {
            gameplayUIManager.ShowSettingsUI();
        });

        button_ToMainMenu.onClick.AddListener(() =>
        {
            gameplayUIManager.ToMainMenu();
        });

    }
}

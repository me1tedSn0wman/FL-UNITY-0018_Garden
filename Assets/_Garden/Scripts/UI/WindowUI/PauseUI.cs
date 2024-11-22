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
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_ResumeGame.onClick.AddListener(() =>
        {
            gameplayUIManager.ResumeGame();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_Settings.onClick.AddListener(() =>
        {
            gameplayUIManager.ShowSettingsUI();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_ToMainMenu.onClick.AddListener(() =>
        {
            gameplayUIManager.ToMainMenu();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

    }
}

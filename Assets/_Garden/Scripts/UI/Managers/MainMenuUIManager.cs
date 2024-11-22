using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private WindowUI upgradesUI;
    [SerializeField] private WindowUI leaderboardUI;
    [SerializeField] private WindowUI settingsUI;

    [Header("Buttons")]
    [SerializeField] private Button button_StartGame;
    [SerializeField] private Button button_Upgrades;
    [SerializeField] private Button button_Leaderboard;
    [SerializeField] private Button button_Settings;

    public void Start()
    {
        button_StartGame.onClick.AddListener(() =>
        {
            GameManager.LOAD_GAMEPLAY_SCENE();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_Upgrades.onClick.AddListener(() =>
        {
            upgradesUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_Leaderboard.onClick.AddListener(() =>
        {
            leaderboardUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_Settings.onClick.AddListener(() =>
        {
            settingsUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });
    }
}

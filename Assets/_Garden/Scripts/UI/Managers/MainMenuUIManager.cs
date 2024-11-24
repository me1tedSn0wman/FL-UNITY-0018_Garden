using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private UpgradesUI upgradesUI;
    [SerializeField] private WindowUI leaderboardUI;
    [SerializeField] private WindowUI settingsUI;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI text_DiamondsValue;

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

        upgradesUI.SetActive(false);
        leaderboardUI.SetActive(false);
        settingsUI.SetActive(false);

        upgradesUI.SpawnUpgradableIcons(GameManager.Instance.gameUpgradesLibrary.GetUpgrades());

        UpdateDiamondsValue(GameManager.Instance.diamonds);

        Subscribe();
    }


    public void UpdateDiamondsValue(int diamonds)
    {
        text_DiamondsValue.text = string.Format("{0:d5}", diamonds);
    }

    public void Subscribe()
    {
        GameManager.Instance.OnDiamondsValueChange += UpdateDiamondsValue;
    }

    public void Unsubscribe()
    {
        GameManager.Instance.OnDiamondsValueChange -= UpdateDiamondsValue;
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }

}

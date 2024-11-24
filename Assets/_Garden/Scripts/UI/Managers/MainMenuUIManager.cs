using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private UpgradesUI upgradesUI;
    [SerializeField] private WindowUI leaderboardUI;
    [SerializeField] private WindowUI settingsUI;
    [SerializeField] private WindowUI infoUI;

    [Header("Textes")]
    [SerializeField] private TextMeshProUGUI text_DiamondsValue;

    [Header("Buttons")]
    [SerializeField] private Button button_StartGame;
    [SerializeField] private Button button_Upgrades;
    [SerializeField] private Button button_Leaderboard;
    [SerializeField] private Button button_Settings;
    [SerializeField] private Button button_Info;

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

        button_Info.onClick.AddListener(() =>
        {
            infoUI.SetActive(true);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        upgradesUI.SetActive(false);
        leaderboardUI.SetActive(false);
        settingsUI.SetActive(false);
        infoUI.SetActive(false);

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

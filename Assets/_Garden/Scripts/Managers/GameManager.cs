using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using YG;

public class GameManager : Soliton<GameManager>
{
    public static bool dataIsLoaded;
    private AudioControlManager audioControlManager;

    public WindowUI loadingFirstScreenUI;

    public AudioLibrary soundLibrary;
    public AudioLibrary musicLibrary;

    public GameplayUpgradesLibrary gameUpgradesLibrary;

    [SerializeField] private int _diamonds;
    public int diamonds {
        get { return _diamonds; }
        private set {
            _diamonds = value;
            OnDiamondsValueChange?.Invoke(value);
        }
    }


    public string leaderboardName;
    public int highScore;

    public event Action<int> OnDiamondsValueChange;

    public override void Awake() { 
        base.Awake();
        dataIsLoaded = false;

#if PLATFORM_WEBGL
        loadingFirstScreenUI.SetActive(true);
#endif
    }

    public void Start()
    {
#if PLATFORM_WEBGL
        StartCoroutine(InitYandex());
#endif        
    }

    IEnumerator InitYandex() { 
        yield return new WaitForSeconds(0.1f);

        YandexGame.GetDataEvent += GetLoadDataYG;
//        YandexGame.RewardVideoEvent += Reward;
        if (!dataIsLoaded) GetLoadDataYG();
        yield return new WaitForSeconds(0.5f);
        YandexGame.GameReadyAPI();
        musicLibrary.PlayLoop("mainMenu");
        loadingFirstScreenUI.SetActive(false);
    }

    private void GetLoadDataYG() {
#if PLATFORM_WEBGL
        dataIsLoaded = true;

        diamonds = YandexGame.savesData.diamonds;
        highScore = YandexGame.savesData.highScore;

        for (int i = 0; i < YandexGame.savesData.upgradeSaveData.Count; i++) {
            gameUpgradesLibrary.SetLevel(
                YandexGame.savesData.upgradeSaveData[i].uid,
                YandexGame.savesData.upgradeSaveData[i].level
                );
        }

        Debug.Log("YG DATA IS LOADED");
#endif
    }

    public void SaveGameData() {
#if PLATFORM_WEBGL
        SaveDataYG();
#endif
    }

    public void SaveDataYG() {
#if PLATFORM_WEBGL
        YandexGame.savesData.diamonds = diamonds;
        YandexGame.savesData.highScore = highScore;

        foreach (GameplayUpgrade upgr in gameUpgradesLibrary.GetUpgrades()) {
            YandexGame.savesData.SetSavedUpgradeLevel(
                upgr.uid,
                upgr.crntLevel
                );
        }

        YandexGame.SaveProgress();
#endif
    }

    public static void LOAD_GAMEPLAY_SCENE() {
        SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
        Instance.musicLibrary.PlayLoop("gameplay");
    }

    public static void LOAD_MAIN_MENU_SCENE() {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        Instance.musicLibrary.PlayLoop("mainMenu");
    }

    public void TryBuyUpgrade(string uid) {
        int price = gameUpgradesLibrary.GetUpgradePrice(uid);
        if (diamonds >= price) {
            diamonds -= price;
            gameUpgradesLibrary.IncreaseCurrentUpgradeLevel(uid);
            SaveGameData();
        }
        
    }

    public void AddDiamonds(int value) {
        diamonds += value;
        SaveGameData();
    }

    public void UpdateHighScore(int score) {
        if (score <= highScore) return;
        highScore = score;
        SaveDataYG();
        YandexGame.NewLeaderboardScores(leaderboardName, score);
    }
}

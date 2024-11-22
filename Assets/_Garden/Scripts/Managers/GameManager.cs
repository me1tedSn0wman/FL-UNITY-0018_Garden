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
}

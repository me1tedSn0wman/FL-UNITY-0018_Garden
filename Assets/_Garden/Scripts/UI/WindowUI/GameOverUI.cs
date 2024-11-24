using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameOverUI : WindowUI
{
    [Header("Buttons")]
    [SerializeField] private Button button_ResumeGame;
    [SerializeField] private Button button_RestartGame;
    [SerializeField] private Button button_ToMainMenu;

    private bool isHaveSecondChance;

    public void Start()
    {
        button_ResumeGame.onClick.AddListener(() =>
        {
            ActivateSecondChance();
            YandexGame.FullscreenShow();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_RestartGame.onClick.AddListener(() =>
        {
            GameplayManager.Instance.Restart();
        });

        button_ToMainMenu.onClick.AddListener(() =>
        {
            GameManager.LOAD_MAIN_MENU_SCENE();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        isHaveSecondChance = true;
    }

    public void ActivateSecondChance() {
        isHaveSecondChance = false;
        button_ResumeGame.gameObject.SetActive(false);
        SetActive(false);
        GameplayManager.Instance.SecondChance();
    }
}

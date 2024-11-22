using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameOverUI : WindowUI
{
    [Header("Buttons")]
    [SerializeField] private Button button_ResumeGame;
    [SerializeField] private Button button_RestartGame;
    [SerializeField] private Button button_ToMainMenu;

    public void Start()
    {
        button_ResumeGame.onClick.AddListener(() =>
        {
            YandexGame.FullscreenShow();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_RestartGame.onClick.AddListener(() =>
        {
            GameManager.LOAD_GAMEPLAY_SCENE();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_ToMainMenu.onClick.AddListener(() =>
        {
            GameManager.LOAD_MAIN_MENU_SCENE();
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

    }
}

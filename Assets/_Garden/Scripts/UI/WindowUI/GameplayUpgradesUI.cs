using UnityEngine;
using UnityEngine.UI;

public class GameplayUpgradesUI : WindowUI
{
    [Header("Upgrades UI")]
    [Header("Buttons")]
    [SerializeField] private Button button_HideGameplayUpgradesLeft;
    [SerializeField] private Button button_HideGameplayUpgradesRight;

    [Header("Content")]
    [SerializeField] private GameObject go_Content;

    [Header("Prefab")]
    [SerializeField] private GameObject prefab_UpgradePrefab;

    public void Start()
    {
        button_HideGameplayUpgradesLeft.onClick.AddListener(() =>
        {
            SetActive(false);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });

        button_HideGameplayUpgradesRight.onClick.AddListener(() =>
        {
            SetActive(false);
            GameManager.Instance.soundLibrary.PlayOneShoot("clickUI");
        });
    }

    public void OnEnable()
    {
        UpdateContent();
    }

    public void UpdateContent() { 
    
    }
}

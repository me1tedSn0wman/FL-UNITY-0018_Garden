using System.Collections.Generic;
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
    [SerializeField] private GameplayUpgradeIconPrefab prefab_UpgradeIconPrefab;

    [Header("Set Dynamically")]
    [SerializeField] private List<GameplayUpgradeIconPrefab> listOfIcons;

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

    public void SpawnUpgradableIcons(GameplayUpgrade[] upgrades) {
        listOfIcons = new List<GameplayUpgradeIconPrefab>();
        for (int i = 0; i < upgrades.Length; i++) {
            GameplayUpgradeIconPrefab newIcon = Instantiate(prefab_UpgradeIconPrefab, go_Content.transform);

            newIcon.SetUpIcon(upgrades[i]);
            listOfIcons.Add(newIcon);
        }
    }
}

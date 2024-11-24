using System.Collections.Generic;
using UnityEngine;

public class UpgradesUI : WindowUI
{
    [Header("Upgrades UI")]
    [Header("Content")]
    [SerializeField] private GameObject go_Content;

    [Header("Prefab")]
    [SerializeField] private GameUpgradeIconPrefab prefab_GameUpgradeIconPrefab;

    [Header("Set Dynamically")]
    [SerializeField] private List<GameUpgradeIconPrefab> listOfIcons;

    public void OnEnable()
    {
        UpdateContent();
    }

    public void UpdateContent() {
        for (int i = 0; i < listOfIcons.Count; i++) {
            listOfIcons[i].CheckUpgrade();
        }
    }

    public void SpawnUpgradableIcons(GameplayUpgrade[] upgrades) {
        listOfIcons = new List<GameUpgradeIconPrefab>();

        for (int i = 0; i < upgrades.Length; i++) {
            GameUpgradeIconPrefab newIcon = Instantiate(prefab_GameUpgradeIconPrefab, go_Content.transform);

            newIcon.SetUpIcon(upgrades[i]);
            listOfIcons.Add(newIcon);
        }
    }


}

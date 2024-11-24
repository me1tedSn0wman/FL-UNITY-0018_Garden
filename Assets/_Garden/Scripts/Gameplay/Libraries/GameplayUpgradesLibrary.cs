using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct UpgradeData {
    public int level;
    public int valuePerLevel;

    public UpgradeData(int level, int value) { 
        this.level = level;
        this.valuePerLevel = value;
    }
}

public class GameplayUpgrade {
    public string uid;
    public string title;
    public string description;

    public int priceBase;
    public int priceStep;

    public Sprite sprite;

    public int crntLevel;
    public int maxLevel;

    public int valuePerLevel;

    public GameplayUpgrade(GameplayUpgradeDef def) {
        this.uid = def.uid;
        this.title = def.title;
        this.description = def.description;

        this.priceBase = def.priceBase;
        this.priceStep = def.priceStep;

        this.sprite = def.sprite;

        this.crntLevel = 0;
        this.maxLevel = def.maxLevel;

        this.valuePerLevel = def.valuePerLevel;
    }

    public int GetUpgradePrice() {
        return priceBase + crntLevel * priceStep;
    }

    public bool IsMaxLevel() {
        return crntLevel == maxLevel;
    }

    public void IncreaseLevel() {
        crntLevel = Mathf.Min(crntLevel + 1, maxLevel);
    }

    public int GetFinalValue() {
        return crntLevel * valuePerLevel;
    }

    public UpgradeData GetUpgradeData() {
        return new UpgradeData(crntLevel, valuePerLevel);
    }
}

public class GameplayUpgradesLibrary : MonoBehaviour
{
    [SerializeField] private GameplayUpgradeDef[] upgradesDef;
    [SerializeField] private Dictionary<string, GameplayUpgrade> dictOfUpgrades;

    private GameplayUpgrade nullGameplayUpgrade;

    public event Action<string> OnUpgradeLevelIncrease;

    public void Awake()
    {
        InitDictionary();
    }

    public void InitDictionary() {
        dictOfUpgrades= new Dictionary<string, GameplayUpgrade>();
        for (int i = 0; i < upgradesDef.Length; i++) {
            GameplayUpgrade newUpgrd = new GameplayUpgrade(upgradesDef[i]);

            dictOfUpgrades.Add(newUpgrd.uid, newUpgrd);
        }
    }

    public GameplayUpgrade GetUpgrade(string uid) {
        if (!dictOfUpgrades.ContainsKey(uid)) {
            Debug.Log("(GameplayUpgradesLibrary) There are no upgrade with that uid: " + uid);
            return nullGameplayUpgrade;
        }
        return dictOfUpgrades[uid];
    }

    public GameplayUpgrade[] GetUpgrades() {
        return dictOfUpgrades.Values.ToArray();
    }

    public int GetUpgradeFinalValue(string uid) {
        if (!dictOfUpgrades.ContainsKey(uid)) {
            Debug.Log("(GameplayUpgradesLibrary) There are no upgrade with that uid: " + uid);
            return 0;
        }
        return dictOfUpgrades[uid].GetFinalValue();
    }

    public int GetUpgradePrice(string uid) {
        if (!dictOfUpgrades.ContainsKey(uid)) {
            Debug.Log("(GameplayUpgradesLibrary) There are no upgrade with that uid: " + uid);
            return 9999;
        }
        return dictOfUpgrades[uid].GetUpgradePrice();
    }

    public void IncreaseCurrentUpgradeLevel(string uid) {
        if (!dictOfUpgrades.ContainsKey(uid))
        {
            Debug.Log("(GameplayUpgradesLibrary) There are no upgrade with that uid: " + uid);
            return;
        }
        dictOfUpgrades[uid].IncreaseLevel();
        OnUpgradeLevelIncrease?.Invoke(uid);
    }

    public void SetLevel(string uid, int level) {
        if (!dictOfUpgrades.ContainsKey(uid))
        {
            Debug.Log("(GameplayUpgradesLibrary) There are no upgrade with that uid: " + uid);
            return;
        }

        dictOfUpgrades[uid].crntLevel = level;
        OnUpgradeLevelIncrease?.Invoke(uid);
    }
}

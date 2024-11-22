using System.Collections.Generic;
using UnityEngine;

public struct GameplayUpgrade {
    public string uid;
    public string title;
    public string description;

    public int priceBase;
    public int priceStep;

    public Sprite sprite;

    public int crntLevel;
    public int maxLevel;

    public GameplayUpgrade(GameplayUpgradeDef def) {
        this.uid = def.uid;
        this.title = def.title;
        this.description = def.description;

        this.priceBase = def.priceBase;
        this.priceStep = def.priceStep;

        this.sprite = def.sprite;

        this.crntLevel = 0;
        this.maxLevel = def.maxLevel;
    }
}

public class GameplayUpgradesLibrary : MonoBehaviour
{
    [SerializeField] private GameplayUpgradeDef[] upgradesDef;
    [SerializeField] private Dictionary<string, GameplayUpgrade> dictOfUpgrades;

    private GameplayUpgrade nullGameplayUpgrade;

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
}

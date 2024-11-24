using UnityEngine;


/// <summary>
/// WHITE
/// </summary>
public class IncomeFlower : Flower
{
    public int baseIncomeCount;
    public float baseTimeBetweenIncomeSpawn;

    [Header("Set Dynamically")]
    
    public int incomeCount;
    [SerializeField] protected float crntTimeIncomeSpawn;
    [SerializeField] protected float timeBetweenIncomeSpawn;

    public override void Update()
    {
        base.Update();
        TrySpawnIncome();
    }

    public virtual void TrySpawnIncome() {
        if (crntTimeIncomeSpawn >= timeBetweenIncomeSpawn) {
            SpawnIncome();
            crntTimeIncomeSpawn = 0f;
            return;
        }
        crntTimeIncomeSpawn += Time.deltaTime;
        return;
    }

    public virtual void SpawnIncome() { 
        GameplayManager.Instance.AddMoney(incomeCount);
    }

    public override void CheckUpgrades(string upgradeUID)
    {
        int incomeAmountFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("whiteIncomeAmount");
        int rateAmountFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("whiteIncomeRate");

        incomeCount = baseIncomeCount + incomeAmountFromUpgrades;
        timeBetweenIncomeSpawn = baseTimeBetweenIncomeSpawn * Mathf.Clamp(1.0f - rateAmountFromUpgrades*0.01f, 0.1f, 1f);
    }
}

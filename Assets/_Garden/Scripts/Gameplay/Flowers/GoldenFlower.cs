public class GoldenFlower : IncomeFlower
{
    public override void SpawnIncome()
    {
        GameplayManager.Instance.AddDiamonds(incomeCount);
    }

    public override void CheckUpgrades(string upgradeUID) {
        incomeCount = baseIncomeCount;
        timeBetweenIncomeSpawn = baseTimeBetweenIncomeSpawn;
    }
}

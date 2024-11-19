using UnityEngine;

public class IncomeFlower : Flower
{
    public int incomeCount;

    [SerializeField] private float crntTimeIncomeSpawn;
    [SerializeField] private float timeBetweenIncomeSpawn;

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

    public void SpawnIncome() { 
        GameplayManager.Instance.AddMoney(incomeCount);
    }
}

using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// PURPLE
/// </summary>
public class AreaFlower : Flower
{
    [SerializeField] protected float baseAreaDamage;
    [SerializeField] protected float baseAreaRadius;

    [SerializeField] private float timeBetweenAreaDamage;
    [SerializeField] private GameObject areaDamageShowGO;
    
    [Header("Set Dynamically")]

    [SerializeField] protected float areaDamage;
    [SerializeField] protected float areaRadius;
    [SerializeField] private Vector3 areaLocalScaleBase;

    [SerializeField] private float crntTimeAreaDamage;
    [SerializeField] private AreaFlowerCollider areaFlowerCollider;
    [SerializeField] private List<Enemy> listOfEnemiesInArea;

    public void Awake()
    {
        areaLocalScaleBase = areaDamageShowGO.transform.localScale;
        areaFlowerCollider = areaDamageShowGO.GetComponent<AreaFlowerCollider>();
        listOfEnemiesInArea = new List<Enemy>();
    }

    public override void Update() {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
            return;
        if (flowerState != FlowerState.None)
            return;
        base.Update();
        TryMakeAreaDamage();
    }

    public virtual void TryMakeAreaDamage() {
        if (crntTimeAreaDamage >= timeBetweenAreaDamage) {
            MakeAreaDamage();
            crntTimeAreaDamage = 0.0f;
            return;
        }
        crntTimeAreaDamage += Time.deltaTime;
        return;
    }

    public virtual void MakeAreaDamage() {
        listOfEnemiesInArea.RemoveAll(enemy => enemy == null);

        for (int i = 0; i < listOfEnemiesInArea.Count; i++) {
            listOfEnemiesInArea[i].ChangeHealth(-areaDamage);
        }
    }

    public void EnemyEntryArea(Enemy enemy) {
        Debug.Log("(AreaFlower) Enemy Entry Area" + enemy.name);

        if (!listOfEnemiesInArea.Contains(enemy)) { 
            listOfEnemiesInArea.Add(enemy);
        }
    }

    public void EnemyLeaveArea(Enemy enemy)
    {
        Debug.Log("(AreaFlower) Enemy Leave Area" + enemy.name);

        if (!listOfEnemiesInArea.Contains(enemy)) {
            return;
        }
        listOfEnemiesInArea.Remove(enemy);
    }

    public override void Death()
    {
        listOfEnemiesInArea.Clear();
        base.Death();
    }

    public override void Subscribe() {
        base.Subscribe();
        areaFlowerCollider.OnEnemyEntryArea += EnemyEntryArea;
        areaFlowerCollider.OnEnemyLeaveArea += EnemyLeaveArea;
    }

    public override void Unsubscribe()
    { 
        areaFlowerCollider.OnEnemyEntryArea -= EnemyEntryArea;
        areaFlowerCollider.OnEnemyLeaveArea -= EnemyLeaveArea;
        base.Unsubscribe();
    }

    public override void CheckUpgrades(string upgradeUID)
    {
        float areaDamageFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("purpleAreaDamage");
        float areaRadiusFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("purpleAreaSize");

        areaDamage = baseAreaDamage * (1 + areaDamageFromUpgrades *0.01f);
        areaRadius = baseAreaRadius * (1 + 0.01f* areaRadiusFromUpgrades);

        areaDamageShowGO.transform.localScale = areaLocalScaleBase * areaRadius;
    }
}

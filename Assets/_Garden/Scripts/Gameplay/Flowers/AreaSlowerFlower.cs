using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BLUE
/// </summary>
public class AreaSlowerFlower : Flower
{
    [SerializeField] protected float baseAreaRadius;

    public float slowedTime;

    [SerializeField] private float timeBetweenSlowedStatus;
    [SerializeField] private GameObject areaFlowerColliderGO;

    [Header("Set Dynamically")]
    [SerializeField] protected float areaRadius;
    [SerializeField] private Vector3 areaLocalScaleBase;

    [SerializeField] private float crntTimeAreaSlower;
    [SerializeField] private AreaFlowerCollider areaFlowerCollider;
    [SerializeField] private List<Enemy> listOfEnemiesInArea;

    public void Awake()
    {
        areaLocalScaleBase = areaFlowerColliderGO.transform.localScale;
        areaFlowerCollider = areaFlowerColliderGO.GetComponent<AreaFlowerCollider>();
        listOfEnemiesInArea = new List<Enemy>();
    }


    public override void Update()
    {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
            return;
        if (flowerState != FlowerState.None)
            return;
        base.Update();
        TryMakeAreaSlower();
    }

    public virtual void TryMakeAreaSlower() {
        if (crntTimeAreaSlower >= timeBetweenSlowedStatus) {
            MakeAreaSlower();
            crntTimeAreaSlower = 0.0f;
            return;
        }
        crntTimeAreaSlower += Time.deltaTime;
        return;
    }

    public virtual void MakeAreaSlower() {
        listOfEnemiesInArea.RemoveAll(enemy => enemy == null);

        for (int i = 0; i < listOfEnemiesInArea.Count; i++) {
            listOfEnemiesInArea[i].SetSlowed(slowedTime);
        }
    }

    public void EnemyEntryArea(Enemy enemy) {
        Debug.Log("(AreaFlower) Enemy Entry Area" + enemy.name);

        if (!listOfEnemiesInArea.Contains(enemy)) {
            listOfEnemiesInArea.Add(enemy);
        }
    }

    public void EnemyLeaveArea(Enemy enemy) {
        Debug.Log("(AreaFlower) Enemy Leave Area" + enemy.name);

        if (!listOfEnemiesInArea.Contains(enemy))
        {
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

    public override void CheckUpgrades(string upgradeUID) {
        int areaRadiusFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("blueAreaSize");

        areaRadius = baseAreaRadius * (1 + 0.01f * areaRadiusFromUpgrades);

        areaFlowerColliderGO.transform.localScale = areaLocalScaleBase * areaRadius;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFlower : Flower
{
    public float areaDamage;
    public float areaRadius;

    [SerializeField] private float timeBetweenAreaDamage;
    [SerializeField] private GameObject areaDamageShowGO;
    
    [Header("Set Dynamically")]
    [SerializeField] private float crntTimeAreaDamage;
    [SerializeField] private Vector3 areaLocalScaleBase;
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
}

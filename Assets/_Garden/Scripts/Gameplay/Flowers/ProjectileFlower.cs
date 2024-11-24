using UnityEngine;
using Utils.PoolControl;

public class ProjectileFlower : Flower
{
    [SerializeField] protected float baseProjectileDamage;
    [SerializeField] protected float baseProjectileSpeed;
    [SerializeField] protected float baseProjectileSize;
    [SerializeField] protected float baseTimeBetweenProjectileSpawns;

    [Header("Prefabs")]
    public GameObject projectilePrefabGO;

    [Header("Set Dynamically")]
    [SerializeField] protected float projectileDamage;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileSize;
    [SerializeField] protected float timeBetweenProjectileSpawns;

    [SerializeField] protected float crntTimeProjectileSpawn;

    public override void Update()
    {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
            return;
        if (flowerState != FlowerState.None)
            return;
        base.Update();
        TrySpawnProjectile();
    }

    public virtual void TrySpawnProjectile() {

        if (crntTimeProjectileSpawn >= timeBetweenProjectileSpawns) {
            SpawnWaveProjectiles();
            crntTimeProjectileSpawn = 0f;
            return;
        }
        crntTimeProjectileSpawn += Time.deltaTime;
        return;
    }

    public virtual void SpawnWaveProjectiles() {
        Vector3 direction = (GameplayManager.Instance.GetNearestEnemyPos() - transform.position);

        SpawnProjectile(direction);
    }

    public virtual void SpawnProjectile(Vector3 direction) {
        Projectile newProjectile = Poolable.TryGetPoolable<Projectile>(projectilePrefabGO);
        newProjectile.damage = projectileDamage;
        newProjectile.speed = projectileSpeed;
        newProjectile.transform.position = transform.position;
        newProjectile.transform.localScale *= projectileSize;
        newProjectile.transform.SetParent(GameplayManager.Instance.projectileAnchor);

        /*
        Vector3 direction = (GameplayManager.Instance.GetNearestEnemyPos() - transform.position);
        newProjectile.direction = new Vector3(direction.x, direction.y, 0).normalized;
        */

        newProjectile.direction = new Vector3(direction.x, direction.y, 0).normalized;

        GameManager.Instance.soundLibrary.PlayOneShoot("fireProjectile");
    }

    public override void CheckUpgrades(string upgradeUID) 
    {
        int projectileSpeedFromUpgr = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("redProjectileSpeed");
        int projectileSizeFromUpgr = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("redProjectileSize");
        int projectileDamageFromUpgr = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("redProjectileDamage");
        int spawnRateFromUpgrades = GameplayManager.Instance.gameplayUpgradesLibrary.GetUpgradeFinalValue("redSpawnRate");

        projectileDamage = baseProjectileDamage + projectileDamageFromUpgr;
        projectileSpeed = baseProjectileSpeed * (1 + 0.01f * projectileSpeedFromUpgr);
        projectileSize = baseProjectileSize * (1 + 0.01f * projectileSizeFromUpgr);
        timeBetweenProjectileSpawns = baseTimeBetweenProjectileSpawns * Mathf.Clamp(1.0f - 0.01f * spawnRateFromUpgrades, 0.1f, 1.0f);
    }
}

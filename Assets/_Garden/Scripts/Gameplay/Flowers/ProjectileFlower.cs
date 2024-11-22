using UnityEngine;
using Utils.PoolControl;

public class ProjectileFlower : Flower
{
    public float projectileDamage;
    public float projectileSpeed;

    public GameObject projectilePrefabGO;


    [SerializeField] private float timeBetweenProjectileSpawns;

    [Header("Set Dynamically")]
    [SerializeField] private float crntTimeProjectileSpawn;

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
            SpawnProjectile();
            crntTimeProjectileSpawn = 0f;
            return;
        }
        crntTimeProjectileSpawn += Time.deltaTime;
        return;
    }

    public virtual void SpawnProjectile() {
        Projectile newProjectile = Poolable.TryGetPoolable<Projectile>(projectilePrefabGO);
        newProjectile.damage = projectileDamage;
        newProjectile.speed = projectileSpeed;
        newProjectile.transform.position = transform.position;
        newProjectile.transform.SetParent(GameplayManager.Instance.projectileAnchor);

        Vector3 direction = (GameplayManager.Instance.GetNearestEnemyPos() - transform.position);
        newProjectile.direction = new Vector3(direction.x, direction.y, 0).normalized;

        GameManager.Instance.soundLibrary.PlayOneShoot("fireProjectile");
    }
}

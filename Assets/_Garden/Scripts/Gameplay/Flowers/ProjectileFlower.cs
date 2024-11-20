using UnityEngine;
using Utils.PoolControl;

public class ProjectileFlower : Flower
{
    public float projectileDamage;
    public float projectileSpeed;

    public GameObject projectilePrefabGO;

    [SerializeField] private float crntTimeProjectileSpawn;
    [SerializeField] private float timeBetweenProjectileSpawns;

    public override void Update()
    {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
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

    public void SpawnProjectile() {
        Projectile newProjectile = Poolable.TryGetPoolable<Projectile>(projectilePrefabGO);
        newProjectile.damage = projectileDamage;
        newProjectile.speed = projectileSpeed;
        newProjectile.transform.position = transform.position;
        newProjectile.transform.SetParent(GameplayManager.Instance.projectileAnchor);

        Vector3 direction = (GameplayManager.Instance.GetNearestEnemyPos() - transform.position);
        newProjectile.direction = new Vector3(direction.x, direction.y, 0).normalized;
    }
}

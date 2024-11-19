using UnityEngine;

public class ProjectileFlower : Flower
{
    public float projectileDamage;
    public float projectileSpeed;

    public Projectile projectilePrefab;

    [SerializeField] private float crntTimeProjectileSpawn;
    [SerializeField] private float timeBetweenProjectileSpawns;

    public override void Update()
    {
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
        Projectile newProjectile = Instantiate(projectilePrefab);
        newProjectile.damage = projectileDamage;
        newProjectile.speed = projectileSpeed;
        newProjectile.transform.position = transform.position;
        newProjectile.transform.SetParent(GameplayManager.Instance.projectileAnchor);
        newProjectile.direction = (GameplayManager.Instance.GetNearestEnemyPos() - transform.position).normalized;
    }
}

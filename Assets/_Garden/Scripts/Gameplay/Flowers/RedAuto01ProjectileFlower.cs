using UnityEngine;

public class RedAuto01ProjectileFlower : ProjectileFlower
{
    public override void SpawnWaveProjectiles() {
        Vector3 direction = GetDirectionToNearestEnemy();

        if (direction == Vector3.zero) {
            return;
        }

        Vector2 dir1= new Vector2(direction.x, direction.y);

        Vector2 dir2 = Quaternion.AngleAxis(15f, Vector3.forward) * dir1;
        Vector2 dir3 = Quaternion.AngleAxis(-15f, Vector3.forward) * dir1;

        SpawnProjectile(dir1);
        SpawnProjectile(dir2);
        SpawnProjectile(dir3);
    }
}

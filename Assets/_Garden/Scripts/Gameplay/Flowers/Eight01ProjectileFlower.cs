using UnityEngine;

public class Eight01ProjectileFlower : ProjectileFlower
{
    public override void SpawnWaveProjectiles() {
        SpawnProjectile(new Vector3( 1,  0, 0));
        SpawnProjectile(new Vector3(-1,  0, 0));
        SpawnProjectile(new Vector3( 0,  1, 0));
        SpawnProjectile(new Vector3( 0, -1, 0));

        SpawnProjectile(new Vector3( 1,  1, 0));
        SpawnProjectile(new Vector3( 1, -1, 0));
        SpawnProjectile(new Vector3(-1,  1, 0));
        SpawnProjectile(new Vector3(-1, -1, 0));
    }
}

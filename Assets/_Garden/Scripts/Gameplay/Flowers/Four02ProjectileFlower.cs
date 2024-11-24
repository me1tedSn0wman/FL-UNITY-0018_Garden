using UnityEngine;

public class Four02ProjectileFlower : ProjectileFlower
{
    public override void SpawnWaveProjectiles() {
        SpawnProjectile(new Vector3( 1,  1, 0));
        SpawnProjectile(new Vector3( 1, -1, 0));
        SpawnProjectile(new Vector3(-1,  1, 0));
        SpawnProjectile(new Vector3(-1, -1, 0));
    }
}

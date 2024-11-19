using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;

    public Vector3 direction;

    public virtual void Update()
    {
        TryMove();
    }

    public virtual void TryMove() {
        transform.position += Time.deltaTime * speed * direction;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
//        Debug.Log(collision.name);
        if (collision.CompareTag("Enemy")) {
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            if (other != null) {
                other.ChangeHealth(-damage);
                DestroyProjectile();
            }
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

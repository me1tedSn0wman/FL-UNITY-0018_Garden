using UnityEngine;
using Utils.PoolControl;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;

    public Vector3 direction;

    [SerializeField] private Vector2 boundXMinMax;
    [SerializeField] private Vector2 boundYMinMax;
    [SerializeField] private Vector3 baseScale;

    public void Awake()
    {
        baseScale = transform.localScale;
    }

    public virtual void Update()
    {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
            return;
        
        TryMove();
        CheckBoundaries();
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
    public void CheckBoundaries()
    {
        if (false
            || transform.position.x <= boundXMinMax.x
            || transform.position.x >= boundXMinMax.y
            || transform.position.y <= boundYMinMax.x
            || transform.position.y >= boundYMinMax.y
            )
            DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        ReturnToPool();
        //        Destroy(gameObject);
    }

    public void ReturnToPool() {
        transform.localScale = baseScale;
        Poolable.TryPool(gameObject);
    }
}

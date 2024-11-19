using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;

    public Vector3 moveAim;


    public float animSpeed = 0.2f;

    private Animator animator;

    public Vector3 pos {
        get { return transform.position; }
    }

    public virtual void Awake() {
        animator = GetComponent<Animator>();
    }

    public virtual void Start() {
        animator.speed = animSpeed;
    }

    public void Update()
    {
        TryMove();
    }

    public void TryMove() {
        transform.position += speed * Time.deltaTime * (moveAim - transform.position).normalized;
    }

    public void Death() {
        GameplayManager.Instance.RemoveEnemyFromList(this);

        Destroy(gameObject);
    }

    public void ChangeHealth(float count) {
        health += count;
        if (health <=0) {
            Death();
        }
    }
}

using UnityEngine;
using Utils.PoolControl;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string _uid;
    public string uid
    {
        get { return _uid; }
    }


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
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
        {
            animator.speed = 0;
            return;
        }

        TryMove();
    }

    public void TryMove() {
        animator.speed = animSpeed;
        transform.position += speed * Time.deltaTime * (moveAim - transform.position).normalized;
    }



    public void ChangeHealth(float count) {
        health += count;
        if (health <=0) {
            Death();
        }
    }

    public void Death()
    {
        GameplayManager.Instance.RemoveEnemyFromList(this);

        ReturnToPool();
    }

    public void ReturnToPool() {
        Poolable.TryPool(gameObject);
    }
}

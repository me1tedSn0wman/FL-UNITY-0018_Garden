using UnityEngine;
using Utils.PoolControl;

public enum EnemyState { 
    None,
    Alive,
    Dead,
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string _uid;
    public string uid
    {
        get { return _uid; }
    }

    private EnemyState _enemyState;
    public EnemyState enemyState { 
        get { return _enemyState; }
        private set { _enemyState = value; }
    }


    public float health;
    public float speed;

    public Vector3 moveAim;


    public float animSpeed = 0.2f;

    private Animator animator;

    public Vector3 pos {
        get { return transform.position; }
    }

    private bool isSlowed;
    [SerializeField] private float crntTimeSlowed;
    [SerializeField] private float timeBeforeRemoveSlowed;

    private SpriteRenderer spriteRend;
    private Color baseColor;

    private Color spriteColor {
        get { return spriteRend.color; }
        set { spriteRend.color = value; }
    }

    public virtual void Awake() {
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        baseColor = spriteColor;
    }

    public virtual void Start() {
        animator.speed = animSpeed;
        enemyState = EnemyState.Alive;
    }

    public void Update()
    {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
        {
            animator.speed = 0;
            return;
        }

        TryMove();
        TryUpdateSlowed();
    }

    public void TryMove() {
        if (enemyState != EnemyState.Alive) {
            return;
        }
        float finalSpeedAnim = !isSlowed ? speed : 0.25f * animSpeed;
        float finalSpeed = !isSlowed ? speed : 0.25f * speed;

        animator.speed = finalSpeedAnim;
        transform.position += finalSpeed * Time.deltaTime * (moveAim - transform.position).normalized;
    }



    public void ChangeHealth(float count) {
        if (enemyState != EnemyState.Alive)
        {
            return;
        }
        health += count;
        if (health <=0) {
            Death();
        }
    }

    public void Death()
    {
        enemyState = EnemyState.Dead;
        GameplayManager.Instance.RemoveEnemyFromList(this);
        GameManager.Instance.soundLibrary.PlayOneShoot("enemyDeath");
        ReturnToPool();
    }

    public void ReturnToPool() {
        enemyState = EnemyState.None;
        Poolable.TryPool(gameObject);
    }

    public void SetSlowed(float time) {

        if (false
            || !isSlowed
            || time > (timeBeforeRemoveSlowed - crntTimeSlowed)
            )
        {
            isSlowed = true;
            timeBeforeRemoveSlowed = time;
            crntTimeSlowed = 0.0f;
            spriteColor = Color.cyan;
        }
    }

    public void TryUpdateSlowed() {
        if (!isSlowed) {
            return;
        }
        if (crntTimeSlowed > timeBeforeRemoveSlowed) {
            isSlowed = false;
            crntTimeSlowed = 0;
            timeBeforeRemoveSlowed = 0;
            spriteColor = baseColor;
            return;
        }
        crntTimeSlowed += Time.deltaTime;
    }
}

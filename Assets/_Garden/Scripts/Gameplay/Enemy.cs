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

    public float baseHealth;
    public float baseSpeed;

    public float healthPerLevel;
    public float speedPerLevel;

    public float animSpeed = 0.2f;

    [Header("Set Dynamically")]
    public int enemyLevel;

    public float health;
    public float speed;
    private bool isSlowed;

    public Vector3 moveAim;

    public Vector3 baseScale;

    public Vector3 pos
    {
        get { return transform.position; }
    }

    [SerializeField] private float crntTimeSlowed;
    [SerializeField] private float timeBeforeRemoveSlowed;

    private Animator animator;
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
        baseScale = transform.localScale;
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
        float finalSpeedAnim = !isSlowed ? animSpeed : 0.25f * animSpeed;
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
        Poolable.TryPool(this.gameObject);
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

    public void SetEnemyLevel(int enemyLevel) {
        enemyState = EnemyState.Alive;
        if ((moveAim - transform.position).x > 0)
        {
            transform.localScale = new Vector3(-1 * baseScale.x, baseScale.y, baseScale.z);
        }
        else {
            transform.localScale = new Vector3( 1 * baseScale.x, baseScale.y, baseScale.z);
        }

        health = baseHealth + enemyLevel * healthPerLevel;
        speed = baseSpeed + enemyLevel * speedPerLevel;
    }
}

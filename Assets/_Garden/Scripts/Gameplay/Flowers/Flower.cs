using UnityEngine;
using Utils.PoolControl;

public enum FlowerState {
    None,
    Dragging,
}


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Flower : MonoBehaviour
{
    [SerializeField] private string _uid;
    [SerializeField] public string uid { 
        get { return _uid; }
    }

    [SerializeField] protected FlowerState flowerState;

    [SerializeField] protected FlowerHealthArea flowerHealthArea;

    public Vector3 immediatePos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public virtual void Start() {
        flowerState = FlowerState.None;
        Subscribe();
    }

    public virtual void Update() {
        if (GameplayManager.Instance.gameplayState != GameplayState.Gameplay)
            return;
        if (flowerState != FlowerState.None)
            return;
    }

    public virtual void FlowerTouchedByEnemy(Enemy enemy)
    {
        if (flowerState == FlowerState.Dragging)
            return;

        enemy.ChangeHealth(-1);
        Death();
    }

    public virtual void OnMouseDown() {
        if (flowerState != FlowerState.None)
            return;
        GameplayManager.Instance.SetSelectedFlower(this);
    }

    public virtual void OnMouseUp() {
        if (flowerState != FlowerState.Dragging)
            return;

        int layerMask = 1 << 6;
        RaycastHit2D hit;

        hit = Physics2D.CircleCast(immediatePos, 0.1f, Vector2.zero, 0f, layerMask);
        if (hit && hit.collider!=null)
        {
            Flower otherFlower = hit.collider.gameObject.GetComponent<Flower>();
            if (otherFlower != null)
            {
                GameplayManager.Instance.TryCombineFlowers(this, otherFlower, otherFlower.immediatePos);
            }
        }
        else
        {
            GameplayManager.Instance.ReleaseFlower(this);
        }
    }

    public void StartDragging() {
        flowerState = FlowerState.Dragging;
        int newLayer = LayerMask.NameToLayer("DraggedFlower");
        gameObject.layer = newLayer;
    }

    public void ReleaseFlower() {
        flowerState = FlowerState.None;
        int newLayer = LayerMask.NameToLayer("Flower");
        gameObject.layer = newLayer;
    }

    public virtual void Death()
    {
        GameplayManager.Instance.RemoveFlowerFromList(this);
        GameManager.Instance.soundLibrary.PlayOneShoot("flowerDeath");
        ReturnToPool();
    }

    public virtual void ReturnToPool()
    {
        Poolable.TryPool(gameObject);
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }

    public virtual void Subscribe() {
        flowerHealthArea.OnEnemyTouchFlower += FlowerTouchedByEnemy;
    }

    public virtual void Unsubscribe()
    {
        flowerHealthArea.OnEnemyTouchFlower -= FlowerTouchedByEnemy;
    }
}

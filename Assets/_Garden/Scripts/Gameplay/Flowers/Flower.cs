using UnityEngine;

public enum FlowerState {
    None,
    Dragging,
}


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Flower : MonoBehaviour
{
    [SerializeField] private FlowerState flowerState;

    public Vector3 immediatePos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public virtual void Start() {
        flowerState = FlowerState.None;
    }

    public virtual void Update() { 
    
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy"))
        {
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            if (other != null)
            {
                other.ChangeHealth(-1);
                Death();
            }
        }
    }

    public void Death()
    {
        GameplayManager.Instance.RemoveFlowerFromList(this);

        Destroy(gameObject);
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
        Debug.Log(gameObject.layer);
    }

    public void ReleaseFlower() {
        flowerState = FlowerState.None;
        int newLayer = LayerMask.NameToLayer("Flower");
        gameObject.layer = newLayer;
        Debug.Log(gameObject.layer);
    }
}

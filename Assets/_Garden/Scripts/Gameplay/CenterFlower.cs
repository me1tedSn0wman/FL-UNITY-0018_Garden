using UnityEngine;

public class CenterFlower : MonoBehaviour
{
    public Vector3 pos
    {
        get { return transform.position; }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) {
            Enemy other = collision.gameObject.GetComponent<Enemy>();

            if (other != null) {
                other.ChangeHealth(-999);
                GameplayManager.Instance.ChangeHealth(-1);
            }
        }
    }
}

using System;
using UnityEngine;

public class AreaFlowerCollider : MonoBehaviour
{
    public event Action<Enemy> OnEnemyEntryArea;
    public event Action<Enemy> OnEnemyLeaveArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Trigger Enter enemy" + collision.name);
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            if (other != null)
            {
                Debug.Log("Send Action event" + collision.name);
                EnemyEntryArea(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger Leave" + collision.name);

        if (collision.CompareTag("Enemy"))
        {
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            if (other != null)
            {
                EnemyLeaveArea(other);
            }
        }
    }

    public void EnemyEntryArea(Enemy enemy) {
        OnEnemyEntryArea?.Invoke(enemy);
    }

    public void EnemyLeaveArea(Enemy enemy)
    {
        OnEnemyLeaveArea?.Invoke(enemy);
    }
}

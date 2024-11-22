using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlowerHealthArea : MonoBehaviour
{
    public event Action<Enemy> OnEnemyTouchFlower;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy other = collision.gameObject.GetComponent<Enemy>();
            if (other != null)
            {
                OnEnemyTouchFlower(other);
            }
        }
    }
}

using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifetime = 5f;
    public PlayerSetup owner;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerSetup enemy = other.GetComponent<PlayerSetup>();
        if (enemy != null && enemy != owner && enemy.isAlive)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

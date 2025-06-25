using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;
    public PlayerSetup owner;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerSetup enemy = other.GetComponent<PlayerSetup>();
        if (enemy != null && enemy.isAlive && enemy != owner)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

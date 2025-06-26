using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 15f;
    public float speed = 10f;
    public float hitThreshold = 0.5f;
    public float maxLifetime = 5f;

    public Transform bulletHitTarget;
    public CharacterSetup shooter;
    private float lifeTimer = 0f;
    private HealthBar targetHealth;

    public void Init(Transform target, CharacterSetup shooter)
    {
        this.bulletHitTarget = target;
        this.shooter = shooter;
        lifeTimer = 0f;
    }

    void Update()
    {
        if (bulletHitTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward the target
        transform.position = Vector3.MoveTowards(transform.position, bulletHitTarget.position, speed * Time.deltaTime);

        // Face the direction of movement
        Vector3 direction = bulletHitTarget.position - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Check if bullet reached target
        if (Vector3.Distance(transform.position, bulletHitTarget.position) < hitThreshold)
        {
            targetHealth = bulletHitTarget.GetComponent<HealthBar>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage, shooter);
            }

            Destroy(gameObject);
        }

        // Destroy bullet after maxLifetime
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }
}
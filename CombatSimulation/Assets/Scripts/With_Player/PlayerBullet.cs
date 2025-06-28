using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public Transform target;
    public PlayerSetup shooter;

    private PlayerHealthBar targetHealth;

    public void Init(Transform targetTransform, PlayerSetup shooterSetup)
    {
        target = targetTransform;
        shooter = shooterSetup;

        if (target != null)
        {
            targetHealth = target.GetComponent<PlayerHealthBar>();

            if (targetHealth == null)
            {
                Debug.LogError($"{target.name} has no PlayerHealthBar.");
            }
        }
        else
        {
            Debug.LogError("Bullet Init() received null target!");
        }

        if (shooter == null)
        {
            Debug.LogError("Bullet Init() received null shooter!");
        }
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < 0.5f)
        {
            if (targetHealth != null && shooter != null)
            {
                targetHealth.TakeDamage(damage, shooter);
            }
            else
            {
                Debug.LogError("Cannot apply damage — targetHealth or shooter is null.");
            }

            Destroy(gameObject);
        }
    }
}

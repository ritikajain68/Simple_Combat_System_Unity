using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public Transform target;
    public PlayerSetup shooter;

    private PlayerHealthBar targetHealth;

    public void Init(Transform targetTransform, PlayerSetup shooterSetup)
    {
        target = targetTransform;
        shooter = shooterSetup;

        if (target == null)
        {
            Debug.LogError("Bullet Init() received null target!");
            Destroy(gameObject);
            return;
        }

        targetHealth = target.GetComponent<PlayerHealthBar>();

        if (targetHealth == null)
        {
            Debug.LogError($"{target.name} has no PlayerHealthBar.");
        }

        if (shooter == null)
        {
            Debug.LogError("Bullet Init() received null shooter!");
        }
    }
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward target
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(direction);
        Debug.DrawRay(transform.position, direction * 2f, Color.yellow);

        // Hit detection
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < 0.5f)
        {
            if (targetHealth != null && shooter != null)
            {
                targetHealth.TakeDamage(damage, shooter);
            }
            else
            {
                Debug.LogWarning("Bullet hit but targetHealth or shooter is missing.");
            }

            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackSpeed = 1.25f;
    public float range = 15f;

    public PlayerSetup playerSetup;

    private void Start()
    {
        // Ensure reference to player setup
        playerSetup = GetComponentInParent<PlayerSetup>();

        // Auto-find firePoint using tag if not assigned in Inspector
        if (firePoint == null)
        {
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("firePoint"))
                {
                    firePoint = child;
                    break;
                }
            }

            if (firePoint == null)
            {
                Debug.LogError("FirePoint with tag 'firePoint' not found in PlayerWeapon children!");
            }
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned in PlayerWeapon!");
        }
    }

    public void Fire(Transform target, PlayerSetup shooter)
    {
        if (target == null || bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Missing target, firePoint, or bulletPrefab. Cannot fire.");
            return;
        }

        PlayerHealthBar targetHealth = target.GetComponent<PlayerHealthBar>();
        if (targetHealth == null || !CanFire(targetHealth)) return;
        
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(target.position - firePoint.position));
        PlayerBullet bullet = bulletObj.GetComponent<PlayerBullet>();

        if (bullet != null)
        {
            bullet.Init(target, shooter);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing PlayerBullet component!");
        }
    }

    public bool CanFire(PlayerHealthBar targetHealth)
    {
        if (targetHealth == null || !targetHealth.CheckIfPlayerAlive()) return false;

        Vector3 origin = firePoint.position;
        Vector3 direction = (targetHealth.transform.position - origin).normalized;
        float distance = Vector3.Distance(origin, targetHealth.transform.position);

        // Optional: show debug ray
        Debug.DrawRay(origin, direction * distance, Color.red, 0.5f);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            return hit.transform == targetHealth.transform;
        }
        return false;
    }
}

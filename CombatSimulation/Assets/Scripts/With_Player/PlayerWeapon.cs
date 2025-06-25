using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 30f;
    public float attackSpeed = 1.25f;
    public float range = 15f;
    public Transform bulletSpawnPoint;

    public void Fire(Transform target)
    {
        if (target == null || bulletSpawnPoint == null) return;

        Vector3 aim = target.position + Vector3.up * 1.2f;
        Vector3 direction = (aim - bulletSpawnPoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
        bullet.transform.localScale = Vector3.one * 1.5f;

        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();
        if (pb != null)
        {
            pb.owner = GetComponentInParent<PlayerSetup>();
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}

using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public float attackSpeed = 1f;
    public float range = 20f;

    [HideInInspector]
    public Transform bulletSpawnPoint;

    public void Fire(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("No target to shoot at.");
            return;
        }

        if (bulletSpawnPoint == null)
        {
            Debug.LogError("Bullet spawn point not set.");
            return;
        }

        Vector3 spawnPos = bulletSpawnPoint.position;
        Vector3 aimPoint = target.position + Vector3.up * 1.2f;
        Vector3 direction = (aimPoint - spawnPos).normalized;

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = spawnPos;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.transform.localScale = Vector3.one * 0.3f;
        bullet.transform.SetParent(null); // detach from weapon

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

        Debug.DrawRay(spawnPos, direction * 5f, Color.green, 2f);
        Debug.Log($"{gameObject.name} fired toward {target.name} at {aimPoint}");
    }
}

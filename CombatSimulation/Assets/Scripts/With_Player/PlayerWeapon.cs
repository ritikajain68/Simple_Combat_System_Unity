using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackSpeed = 1.25f;
    public float range = 15f;
    public Transform bulletSpawnPoint;
    public PlayerSetup playerSetup;    

    public void Fire(Transform target, PlayerSetup shooter)
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Bullet prefab or firePoint is null.");
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        PlayerBullet bullet = bulletObj.GetComponent<PlayerBullet>();

        if (bullet != null)
        {
            bullet.Init(target, shooter);
        }
    }

    private void Start()
    {
        playerSetup = GetComponent<PlayerSetup>();
        if (playerSetup == null)
        {
            Debug.LogError("PlayerSetup component not found on PlayerWeapon!");
        }

        if (bulletSpawnPoint == null)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("firePoint"))
                {
                    bulletSpawnPoint = child;
                    break;
                }
            }

            if (bulletSpawnPoint == null)
            {
                Debug.LogError("firePoint with tag not found in PlayerWeapon!");
            }
        }
    }


    public bool CanFire(PlayerHealthBar targetHealth)
    {
        if (targetHealth == null || !targetHealth.CheckIfPlayerAlive()) return false;

        float distance = Vector3.Distance(transform.position, targetHealth.transform.position);
        return distance <= range;
    }
}
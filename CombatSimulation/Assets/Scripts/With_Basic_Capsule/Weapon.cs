using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackSpeed = 1f;
    public float range = 8f;
    public CharacterSetup characterSetup;

    private void Start()
    {       
        characterSetup = GetComponent<CharacterSetup>();
        if (characterSetup == null)
        {
            Debug.LogError("CharacterSetup component not found on Weapon!");
            return;
        }

        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null)
            {
                Debug.LogError("FirePoint not found in Weapon!");
            }
        }
    }

    public void Fire(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null, cannot fire.");
            return;
        }

        HealthBar targetHealth = target.GetComponent<HealthBar>();
        if (targetHealth == null || !CanFire(targetHealth))
        {
            Debug.LogWarning("Cannot fire at target, either null or not in range.");
            return;
        }
        // Instantiate bullet and set its properties
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned in Weapon!");
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(target, transform.root.GetComponent<CharacterSetup>());
    }

    public bool CanFire(HealthBar targetHealth)
    {
        if (targetHealth == null || !targetHealth.CheckIfPlayerAlive()) return false;

        Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0);
        Vector3 direction = (targetHealth.transform.position - rayOrigin).normalized;
        float distance = Vector3.Distance(rayOrigin, targetHealth.transform.position);

        Debug.DrawRay(rayOrigin, direction * distance, Color.red); // Always draw ray

        if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, distance))
        {
            if (hit.transform == targetHealth.transform)
            {
                return true;
            }
        }
        return false;
    }
}
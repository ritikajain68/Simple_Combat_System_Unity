using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackSpeed = 1f;
    public float range = 8f;

    public void Fire(Transform target)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(target, transform.root.GetComponent<CharacterSetup>());
    }
}

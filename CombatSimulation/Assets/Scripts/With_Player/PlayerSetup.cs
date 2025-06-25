using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerSetup : MonoBehaviour
{
    public float maxHealth = 1000f;
    public float health ;
    public float speed = 3.5f;
    public bool isAlive = true;

    public Animator animator;
    public NavMeshAgent agent;
    public GameObject weaponPrefab;
    public PlayerWeapon weapon;
    private GameObject weaponInstance;
    private Transform bulletSpawnPoint;
    private Transform target;

    void Start()
    {        
        health = maxHealth;
        agent.speed = speed;

        AttachWeaponToHand();
        if (weapon == null)
        {
            Debug.LogError(" Weapon not attached properly.");
            return;
        }
        InvokeRepeating(nameof(FindTarget), 0f, 1f);
        float randomDelay = Random.Range(0f, weapon.attackSpeed);
        InvokeRepeating(nameof(Attack), randomDelay, weapon.attackSpeed);
    }

    void Update()
    {
        if (!isAlive || target == null) return;

        agent.SetDestination(target.position);

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        if (Vector3.Distance(transform.position, target.position) <= weapon.range)
        {
            // Face target
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    void FindTarget()
    {
        target = PlayerBattleManager.Instance.GetRandomTarget(this);
    }

    void Attack()
    {
        if (!isAlive || target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= weapon.range)
        {
            animator.SetTrigger("isShooting");
            weapon.Fire(target);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0f && isAlive)
        {
            isAlive = false;
            gameObject.SetActive(false);
            transform.parent?.gameObject.SetActive(false); // safely disables parent container

            PlayerBattleManager.Instance.CheckBattleState();
            Debug.Log($"{gameObject.name} took fatal damage.");
        }
    }

    void AttachWeaponToHand()
    {        
        Transform hand = GetComponentsInChildren<Transform>()
                         .FirstOrDefault(t => t.CompareTag("WeaponSocket"));

        if (hand != null && weaponPrefab != null)
        {
            weaponInstance = Instantiate(weaponPrefab, hand);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;

            weapon = weaponInstance.GetComponent<PlayerWeapon>();

            bulletSpawnPoint = weaponInstance.transform.Find("Barrel");
            if (bulletSpawnPoint != null)
                weapon.bulletSpawnPoint = bulletSpawnPoint;
            else
                Debug.LogWarning(" Barrel not found on weapon prefab.");
        }
        else
        {
            Debug.LogError("WeaponSocket or weaponPrefab is missing.");
        }
    }
}

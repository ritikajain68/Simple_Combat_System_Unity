using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerSetup : MonoBehaviour
{
    public float health = 100f;
    public float speed = 3.5f;
    public bool isAlive = true;
    public bool isPlayerMesh = false;

    public GameObject weaponPrefab;
    private GameObject weaponInstance;
    private NavMeshAgent agent;
    private Transform target;
    public PlayerWeapon weapon;

    private Transform bulletSpawnPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = speed;

        if (isPlayerMesh)
        {
            AttachWeaponToHand();
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on the NavMesh!");
            return;
        }

        InvokeRepeating(nameof(FindTarget), 0f, 1f);
        InvokeRepeating(nameof(Attack), 1f, weapon?.attackSpeed ?? 1f);
    }

    void Update()
    {
        if (!isAlive || target == null || agent == null) return;

        agent.SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) <= weapon.range)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    public void FindTarget()
    {
        target = PlayerBattleManager.Instance.GetRandomTarget(this);
    }

    public void Attack()
    {
        if (!isAlive || target == null) return;

        if (Vector3.Distance(transform.position, target.position) <= weapon.range)
        {
            weapon.Fire(target);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f && isAlive)
        {
            isAlive = false;
            gameObject.SetActive(false);
            PlayerBattleManager.Instance.CheckBattleState();
        }
    }

    void AttachWeaponToHand()
    {
        Transform hand = GetComponentsInChildren<Transform>()
                         .FirstOrDefault(t => t.CompareTag("WeaponSocket"));

        if (hand == null)
        {
            Debug.LogError("WeaponSocket tag not found.");
            return;
        }

        weaponInstance = Instantiate(weaponPrefab, hand);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;

        bulletSpawnPoint = weaponInstance.transform.Find("Barrel");

        if (bulletSpawnPoint == null)
        {
            Debug.LogError("Barrel not found in weapon.");
            return;
        }

        weapon = weaponInstance.GetComponent<PlayerWeapon>();
        if (weapon != null)
        {
            weapon.bulletSpawnPoint = bulletSpawnPoint;
        }
    }
}

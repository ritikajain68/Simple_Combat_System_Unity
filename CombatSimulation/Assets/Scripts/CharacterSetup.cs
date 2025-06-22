using UnityEngine;
using UnityEngine.AI;

public class CharacterSetup : MonoBehaviour
{
    public float health = 100f;
    public float speed = 3.5f;
    public Weapon weapon;
    public bool isAlive = true;

    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on the NavMesh!");
            return;
        }

        weapon = GetComponentInChildren<Weapon>();
        InvokeRepeating(nameof(FindTarget), 0f, 1f);
        InvokeRepeating(nameof(Attack), 1f, weapon.attackSpeed);
    }

    void Update()
    {
        if (!isAlive || target == null) return;

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
        target = BattleManager.Instance.GetRandomTarget(this);
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
            BattleManager.Instance.CheckBattleState();
        }
    }
}

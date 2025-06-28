using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerSetup : MonoBehaviour
{
    public PlayerSetup opponentCharacter;
    public PlayerAnimator animator;

    public PlayerHealthBar characterHealthBar;
    public PlayerMovement movement;
    public PlayerWeapon weapon;
    public PlayerStatsData playerStatsData = new();

    [Header("Stats")]
    public float TargetRange = 10f;
    public bool isTargetInRange = false;
    private float lastFireTime = 0f;
    public bool isAlive = true;
    
    void Start()
    {
        movement ??= GetComponent<PlayerMovement>();
        characterHealthBar ??= GetComponent<PlayerHealthBar>();
        weapon ??= GetComponent<PlayerWeapon>();
        animator ??= GetComponent<PlayerAnimator>();  
        FindNewTarget();

        // health = maxHealth;
        // isAlive = true;

        // agent ??= GetComponent<NavMeshAgent>();
        // animator ??= GetComponentInChildren<Animator>();

        // if (!agent || !animator || !movement)
        // {
        //     Debug.LogError("Missing required components.");
        //     return;
        // }

        // AttachWeaponToHand();
        // if (weapon == null)
        // {
        //     Debug.LogError("Weapon not attached properly.");
        //     return;
        // }

        // movement.speed = agent.speed;

        // InvokeRepeating(nameof(FindTarget), 0f, 1f);
        // float delay = Random.Range(0f, weapon.attackSpeed);
        // InvokeRepeating(nameof(Attack), delay, weapon.attackSpeed);
    }

    void Update()
    {
        if (!isAlive || PlayerSpawner.IsGameOver) return;

        if (opponentCharacter == null || !opponentCharacter.isAlive)
        {
            FindNewTarget();
        }

        if (opponentCharacter != null && opponentCharacter.characterHealthBar.CheckIfPlayerAlive())
        {
            isTargetInRange = weapon.CanFire(opponentCharacter.characterHealthBar);

            if (isTargetInRange)
            {
                Debug.Log($"{gameObject.name} is in range to attack {opponentCharacter.name}");
                Attack();
            }
            else
            {
                Debug.Log($"{gameObject.name} is moving towards {opponentCharacter.name}");
                MoveTowardsTarget();
            }
        }
    }

    public void FindNewTarget()
    {
        GameObject newTarget = PlayerSpawner.Instance.GetRandomTarget(this);
        if (newTarget != null && newTarget != gameObject)
        {
            opponentCharacter = newTarget.GetComponent<PlayerSetup>();
            //Debug.Log($"{gameObject.name} targets {opponentCharacter.name}");
        }
        else
        {
            opponentCharacter = null;
            //Debug.Log($"{gameObject.name} found no valid target.");
        }
    }


    public void MoveTowardsTarget()
    {
        movement.targetPosition = opponentCharacter.transform;
        animator?.SetWalking(true);
    }

    public void Attack()
    {
        Debug.Log($"{gameObject.name} attacking {opponentCharacter.name}");
        if (Time.time - lastFireTime >= weapon.attackSpeed)
        {
            if (movement.navMeshAgent.velocity.magnitude < 0.1f)
            {
                animator?.SetWalking(false);
            }
            animator?.TriggerShoot();

            weapon.Fire(opponentCharacter.transform, this);
            lastFireTime = Time.time;
        }
    }

    // void Attack()
    // {
    //     if (!isAlive || target == null) return;

    //     if (weapon.CanFire(target))
    //     {
    //         animator.SetTrigger("isShooting");
    //         weapon.Fire(target);
    //     }
    // }

    // public void TakeDamage(float amount)
    // {
    //     health -= amount;
    //     health = Mathf.Clamp(health, 0, maxHealth);

    //     if (health <= 0f && isAlive)
    //     {
    //         isAlive = false;
    //         Debug.Log($"{gameObject.name} died.");
    //         gameObject.SetActive(false);
    //         transform.parent?.gameObject.SetActive(false);
    //         PlayerBattleManager.Instance.CheckBattleState();
    //     }
    // }

    // void AttachWeaponToHand()
    // {
    //     Transform hand = GetComponentsInChildren<Transform>()
    //                      .FirstOrDefault(t => t.CompareTag("WeaponSocket"));

    //     if (hand == null || weaponPrefab == null)
    //     {
    //         Debug.LogError("WeaponSocket or weaponPrefab missing.");
    //         return;
    //     }

    //     weaponInstance = Instantiate(weaponPrefab, hand);
    //     weaponInstance.transform.localPosition = Vector3.zero;
    //     weaponInstance.transform.localRotation = Quaternion.identity;

    //     weapon = weaponInstance.GetComponent<PlayerWeapon>();
    //     bulletSpawnPoint = weaponInstance.transform.Find("Barrel");

    //     if (bulletSpawnPoint != null)
    //         weapon.bulletSpawnPoint = bulletSpawnPoint;
    //     else
    //         Debug.LogWarning("Barrel not found on weapon prefab.");
    // }

    [System.Serializable]
    public class PlayerStatsData
    {
        public string playerName;
        public int killCount;

        public void AddKill()
        {
            killCount++;
            Debug.Log($"{playerName} has {killCount} kills.");
        }
    }
}

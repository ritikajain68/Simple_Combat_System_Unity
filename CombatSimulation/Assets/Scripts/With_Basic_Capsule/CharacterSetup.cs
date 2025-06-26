using UnityEngine;

public class CharacterSetup : MonoBehaviour
{
    public CharacterSetup opponentCharacter;
    public HealthBar characterHealthBar;
    public CharacterMovement characterMovement;
    public Weapon weapon;
    public CharacterStatsData characterStatsData = new();

    [Header("Target Settings")]
    public float TargetRange = 10f;
    public bool isTargetInRange = false;
    private float lastFireTime = 0f;

    public bool isAlive = true;

    void Start()
    {
        characterMovement ??= GetComponent<CharacterMovement>();
        characterHealthBar ??= GetComponent<HealthBar>();
        weapon ??= GetComponentInChildren<Weapon>();
        FindNewTarget();
    }

    void Update()
    {
        if (!isAlive) return;

        if (opponentCharacter == null || !opponentCharacter.isAlive)
        {
            FindNewTarget();
        }

        if (opponentCharacter != null && opponentCharacter.characterHealthBar.CheckIfPlayerAlive())
        {
            isTargetInRange = weapon.CanFire(opponentCharacter.characterHealthBar);

            if (isTargetInRange)
            {
                Attack();
            }
            else
            {
                MoveTowardsTarget();
            }
        }
    }

    public void FindNewTarget()
    {
        GameObject newTarget = Spawner.Instance.GetRandomTarget(this);
        if (newTarget != null && newTarget != gameObject)
        {
            opponentCharacter = newTarget.GetComponent<CharacterSetup>();
            Debug.Log($"{gameObject.name} targets {opponentCharacter.name}");
        }
        else
        {
            opponentCharacter = null;
            Debug.Log($"{gameObject.name} found no valid target.");
        }
    }

    public void MoveTowardsTarget()
    {
        characterMovement.targetPosition = opponentCharacter.transform;
    }

    public void Attack()
    {
        if (Time.time - lastFireTime >= weapon.attackSpeed)
        {
            weapon.Fire(opponentCharacter.characterHealthBar.transform);
            lastFireTime = Time.time;
        }
    }

    [System.Serializable]
    public class CharacterStatsData
    {
        public string characterName;
        public int KillCount;
    }

    [System.Serializable]
    public class CharacterStats : MonoBehaviour
    {
        public CharacterStatsData characterStatsData = new();

        public void AddKill()
        {
            characterStatsData.KillCount++;
            Debug.Log($"{gameObject.name} has {characterStatsData.KillCount} kills.");
        }
    }
}

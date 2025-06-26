using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    [Header("Character Movement Settings")]
    private CharacterSetup characterSetup;
    public float speed = 3f;
    public float rotationSpeed = 1200f;
    private float stuckTimer = 0f;
    private int stuckAttempts = 0;

    public NavMeshAgent navMeshAgent;
    public Transform targetPosition;

    private Vector3 lastTargetPosition;

    void Start()
    {
        characterSetup ??= GetComponent<CharacterSetup>();

        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on the NavMesh!");
            return;
        }

        navMeshAgent.autoRepath = true;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        AddPlayerMovement();
        lastTargetPosition = Vector3.zero;
    }

    void Update()
    {
        if (navMeshAgent.velocity.magnitude < 0.1f)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer > 3f)
            {
                stuckAttempts++;
                stuckTimer = 0f;

                if (stuckAttempts < 3)
                {
                    Debug.Log($"{gameObject.name} is stuck, reassigning new target...");
                    characterSetup?.FindNewTarget();
                }
                else
                {
                    Debug.Log($"{gameObject.name} is still stuck. Moving randomly.");
                    MoveToRandomNearbyPosition();
                    stuckAttempts = 0;
                }
            }
        }
        else
        {
            stuckTimer = 0f;
            stuckAttempts = 0;
        }

        if (targetPosition != null && Vector3.Distance(lastTargetPosition, targetPosition.position) > 0.1f)
        {
            SetTargetPosition(targetPosition);
            lastTargetPosition = targetPosition.position;
        }
    }

    public void AddPlayerMovement()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = rotationSpeed;
        navMeshAgent.acceleration = 10f;
        navMeshAgent.updateRotation = true;
        navMeshAgent.updatePosition = true;
    }

    public void SetTargetPosition(Transform target)
    {
        if (target == null || navMeshAgent == null || !navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning("Invalid target or NavMeshAgent not ready.");
            return;
        }

        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(target.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(target.position);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} can't find path to {target.name}.");
        }
    }

    private void MoveToRandomNearbyPosition()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized * 5f;

        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }
}

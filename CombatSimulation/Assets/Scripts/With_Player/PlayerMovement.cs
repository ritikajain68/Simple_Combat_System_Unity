using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float rotationSpeed = 1200f;

    public NavMeshAgent navMeshAgent;
    public Transform targetPosition;

    private Vector3 lastTargetPosition;
    private float stuckTimer = 0f;
    private int stuckAttempts = 0;

    private PlayerSetup playerSetup;

    void Start()
    {
        playerSetup = GetComponent<PlayerSetup>();
        navMeshAgent ??= GetComponent<NavMeshAgent>();
        animator ??= GetComponent<Animator>();

        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on the NavMesh!");
            return;
        }

        AddPlayerMovement();
        lastTargetPosition = Vector3.zero;
    }
    void Update()
    {
        if (PlayerSpawner.IsGameOver || targetPosition == null)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("isWalking", false);
            return;
        }

        // Animation: walking or idle
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        // Stuck detection
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
                    playerSetup?.FindNewTarget();
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

        // Recalculate path if target has moved
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

        navMeshAgent.autoRepath = true;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        //navMeshAgent.updateRotation = true;
        //navMeshAgent.updatePosition = true;
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
            SmoothRotateTowards(target.position);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} can't find path to {target.name}.");
        }
    }

    private void MoveToRandomNearbyPosition()
    {
        // Vector3 randomDirection = Random.insideUnitSphere * 5f + transform.position;
        // NavMeshHit hit;
        // if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        // {
        //     navMeshAgent.SetDestination(hit.position);
        //     SmoothRotateTowards(hit.position);
        // }

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
            SmoothRotateTowards(hit.position);
        }
    }

    private void SmoothRotateTowards(Vector3 lookAtTarget)
    {
        Vector3 direction = (lookAtTarget - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
    public void SetTarget(Transform target)
    {
        if (target == null) return;
        targetPosition = target;
    }
}

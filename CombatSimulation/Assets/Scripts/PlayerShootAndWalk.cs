using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerShootAndWalk : MonoBehaviour
{
    private Animator animator;
    public CharacterController controller;
    private PlayerWeapon playerWeapon;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 720f;

    [Header("Input Settings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode shootKey = KeyCode.Space;

    [Header("Animation Settings")]
    public string speedParam = "Speed";
    public string shootTrigger = "isShooting";

    [Header("Shoot Target")]
    public Transform targetPosition;  // Assign in inspector to test bullet shoot

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();

        if (playerWeapon == null)
            Debug.LogError("PlayerWeapon not found in children!");
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis(horizontalAxis);
        float v = Input.GetAxis(verticalAxis);
        Vector3 move = new Vector3(h, 0, v);

        // Rotate if moving
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move character
        controller.SimpleMove(move.normalized * moveSpeed);

        // Update animator blend
        animator.SetFloat(speedParam, move.magnitude * moveSpeed);
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(shootKey))
        {
            animator.SetTrigger(shootTrigger);

            // Fire bullet toward dummy target if assigned
            if (targetPosition != null)
            {
                playerWeapon.Fire(targetPosition, null); // shooter can be null for test
            }
            else
            {
                Debug.LogWarning("Dummy target not assigned — bullet won't fire.");
            }
        }
    }
}

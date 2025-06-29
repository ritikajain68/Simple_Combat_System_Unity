using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerTestAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 720f;

    [Header("Input Settings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public KeyCode shootKey = KeyCode.Space;

    [Header("Animation Settings")]
    public bool isShootingBlendMode = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movement Input
        float h = Input.GetAxis(horizontalAxis);
        float v = Input.GetAxis(verticalAxis);
        Vector3 move = new Vector3(h, 0, v);

        // Rotate toward movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move character
        controller.SimpleMove(move.normalized * moveSpeed);

        // Update animation
        float speed = move.magnitude * moveSpeed;

        if (isShootingBlendMode)
        {
            animator.SetFloat("Speed", speed); // blend tree
        }
        else
        {
            animator.SetBool("isWalking", speed > 0.1f); // bool-based
        }

        // Trigger shoot
        if (Input.GetKeyDown(shootKey))
        {
            animator.SetTrigger("isShooting");
        }
    }
}

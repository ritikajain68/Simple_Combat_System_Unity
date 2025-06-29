using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public bool isShootingAnim = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animator not found on player!");
        }
    }
    public void SetMovementSpeed(float speed)
    {
        animator?.SetFloat("Speed", speed, 0.15f, Time.deltaTime);
    }

    public void SetWalking(bool isWalking)
    {
        animator?.SetBool("isWalking", isWalking);
    }

    public void TriggerShoot(string animName)
    {
        animator?.SetTrigger(animName);
    }

    public void SetMovementBlend(float speed)
    {
        animator?.SetFloat("Speed", speed);
    }
}

using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animator not found on player!");
        }
    }

    public void SetWalking(bool isWalking)
    {
        animator?.SetBool("isWalking", isWalking);
    }

    public void TriggerShoot()
    {
        animator?.SetTrigger("isShooting");
    }
}

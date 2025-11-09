using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cactus : MonoBehaviour
{
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isIdling", true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdling", false);
        }
    }

    void OnAttacked()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdling", true);
    }
}

using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Animator))]
public class FallingLeaf : MonoBehaviour
{
    Animator animator;
    float startAt;
    bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0.0f;
        startAt = Time.time + Random.Range(1.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFalling && Time.time > startAt)
        {
            animator.speed = 1.0f;
            isFalling = true;
            animator.SetTrigger("fall");
        }
    }

    void OnFalled()
    {
        var next = Random.Range(0.0f, 2.0f);
        startAt = Time.time + next;
        isFalling = false;
    }
}

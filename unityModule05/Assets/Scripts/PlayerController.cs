using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float jumpPower = 1.0f;
    public float movePower = 1.0f;
    public float hp = 3.0f;
    public UnityEvent defeated;

    private Rigidbody2D rb;
    private Animator animator;
    private bool leftRequested = false;
    private bool rightRequested = false;
    private bool jumpRequested = false;
    private bool isJumping = false;
    private bool isAttacking = false;

    private Vector3 initialPosition;
    public bool isAlive = true;
    private bool inputLock = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isJumping = false;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isAlive)
            return;
        InputRequest();
        ProcessAnimation();
    }

    void FixedUpdate()
    {
        if (!isAlive)
            return;
        InputControl();
    }

    void InputControl()
    {
        if (inputLock)
            return;
        if (leftRequested)
        {
            MoveLeft();
        }
        if (rightRequested)
        {
            MoveRight();
        }
        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }
    }

    void InputRequest()
    {
        leftRequested = Input.GetKey("a");
        rightRequested = Input.GetKey("d");
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            jumpRequested = true;
        if (Input.GetKeyDown(KeyCode.V) && !isAttacking)
            isAttacking = true;
    }

    void ProcessAnimation()
    {
        if (UpdateAnimation())
        {
            animator.SetBool("isAnyAction", true);
        }
        else
        {
            animator.SetBool("isAnyAction", false);
        }
    }

    bool UpdateAnimation()
    {
        animator.SetFloat("xSpeed", rb.linearVelocity.x);
        animator.SetFloat("ySpeed", rb.linearVelocity.y);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isAttack", isAttacking);
        if (isJumping || isAttacking)
            return true;
        return false;
    }

    void MoveRight()
    {
        if (rb.linearVelocity.x < 5f)
            rb.AddForce(Vector2.right * movePower);
    }

    public void Respawn()
    {
        StopMovement();
        hp = 3.0f;
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject respawn in respawns)
        {
            transform.position = respawn.transform.position;
            break;
        }
        if (animator)
        {
            animator.Rebind();
            animator.Play("Respawn");
            animator.Update(0f);
        }
        else
        {
            StartMovement();
        }
        ResetRequest();
    }

    void OnAttack()
    {
        inputLock = true;
    }

    void OnAttacked()
    {
        animator.SetBool("isAttack", false);
        isAttacking = false;
        inputLock = false;
        ResetRequest();
    }

    void ResetRequest()
    {
        jumpRequested = false;
        isAttacking = false;
    }

    void OnDefeat()
    {
        StopMovement();
    }

    void OnDefeated()
    {
        defeated.Invoke();
    }

    public void StopMovement()
    {
        isAlive = false;
        if (rb != null)
            rb.linearVelocity = new Vector2(.0f, .0f);
        ResetRequest();
    }

    public void StartMovement()
    {
        isAlive = true;
    }

    void OnRespawned()
    {
        StartMovement();
    }

    void MoveLeft()
    {
        if (rb.linearVelocity.x > -5f)
            rb.AddForce(Vector2.left * movePower);
    }
    
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        isJumping = true;
    }

    void Knockback()
    {
        float knockbackPower = 5.0f;
        rb.AddForce(new Vector2(-knockbackPower, knockbackPower), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (other.contacts.Any(c => c.normal.y > 0.5f))
            {
                isJumping = false;
                ResetRequest();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAlive && other.gameObject.CompareTag("EnemyAttack"))
        {
            hp -= 1.0f;
            animator.Play("Take Damage");
            Knockback();
            if (hp <= 0.0f)
            {
                animator.Play("Defeated");
            }
        }

        if (isAlive && other.gameObject.CompareTag("Collectable"))
        {
            GameManager.instance.AddItemCount();
            Destroy(other.gameObject);
        }
    }
}

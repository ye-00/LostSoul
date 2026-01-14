using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D rb;
    public int maxHealth = 10;
    public Text health;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float JumpHeight = 5f;

    private float movement;
    private bool facingRight = true;

    [Header("Ground")]
    public bool isGround = true;
    private bool groundedByCollision = false;
    private bool wasInAir = false;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Jump")]
    public int maxJumps = 2;
    private int jumpCount = 0;

    [Header("Ground Slam")]
    public float groundSlamSpeed = -12f;
    private bool hasSlammed = false;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask attackLayer;

    public float attackDamage = 10f;
    public float slamDamage = 25f;

    // 🔥 MULTI-HIT SUPPORT
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    void Update()
    {
        if (FindFirstObjectByType<GameManager>().isGameActive == false)
        if (maxHealth <=0)
        {
            Die();
        }

        health.text = maxHealth.ToString();


        // ================= MOVE =================
        movement = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Run", Mathf.Abs(movement));

        // ================= FLIP =================
        if (movement < 0 && facingRight) Flip();
        else if (movement > 0 && !facingRight) Flip();

        if (!isGround)
            wasInAir = true;

        // ================= JUMP =================
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                jumpCount = 1;
                isGround = false;
                groundedByCollision = false;
                hasSlammed = false;

                DoJump();
                animator.SetBool("Jump", true);
            }
            else if (jumpCount == 1 && maxJumps >= 2)
            {
                jumpCount = 2;
                DoJump();
                animator.SetBool("Jump", false);
                animator.SetTrigger("DoubleJump");
            }
        }

        // ================= FALL =================
        bool inAirAttack =
            animator.GetCurrentAnimatorStateInfo(0).IsName("Jump-Attack") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("GroundSlam");

        if (!isGround && rb.linearVelocity.y < -0.1f && !inAirAttack && !hasSlammed)
            animator.SetBool("Falling", true);
        else
            animator.SetBool("Falling", false);

        // ATTACK 1
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isGround)
            {
                animator.SetTrigger("Attack");
            }
            else if (jumpCount == 1 && !hasSlammed)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Falling", false);
                animator.SetTrigger("Jump-Attack");
            }
        }

        // ATTACK 2
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isGround)
            {
                animator.SetTrigger("Attack-2");
            }
            else if (!hasSlammed)
            {
                hasSlammed = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, groundSlamSpeed);
                animator.SetBool("Jump", false);
                animator.SetBool("Falling", false);
                animator.SetTrigger("GroundSlam");
            }
        }
    }

    void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
    }

    // ================= ANIMATION EVENTS =================

    // 🔥 PANGGIL DI FRAME PEDANG KENA
    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
        if (collInfo.gameObject.GetComponent<EnemyPatrol>() != null)
            {
                collInfo.gameObject.GetComponent<EnemyPatrol>().TakeDamage(1);
                 Camera.main.GetComponent<CameraShake>().Shake();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        return;
        {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    // 🔥 PANGGIL DI FRAME AWAL ANIMASI ATTACK
    public void ResetHitEnemies()
    {
        hitEnemies.Clear();
    }

    // ================= COLLISION =================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                if (!isGround && wasInAir)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundSlam"))
                        animator.SetTrigger("SlamEnd");
                    else
                        animator.SetTrigger("Landing");
                }

                isGround = true;
                groundedByCollision = true;
                wasInAir = false;
                jumpCount = 0;
                hasSlammed = false;

                animator.SetBool("Jump", false);
                animator.SetBool("Falling", false);
                return;
            }
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);
    }


       public void TakeDamage(int damage)
    {
        if (maxHealth <= 0 )
        {
            return;
        }
        maxHealth -= damage;

    }

    void Die()
    {
        Debug.Log("player die");
        FindAnyObjectByType<GameManager>().isGameActive = false;
        Destroy(this.gameObject);
    }

    void CheckGround()
{
    bool grounded = Physics2D.OverlapCircle(
        groundCheck.position,
        groundCheckRadius,
        groundLayer
    );

    // BARU NYENTUH TANAH (LANDING)
    if (grounded && !isGround)
    {
        Debug.Log($"Ground:{isGround} JumpCount:{jumpCount}");
        if (wasInAir)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundSlam"))
                animator.SetTrigger("SlamEnd");
            else
                animator.SetTrigger("Landing");
        }

        isGround = true;
        wasInAir = false;

        jumpCount = 0;      // ✅ reset HANYA saat landing
        hasSlammed = false;

        animator.SetBool("Jump", false);
        animator.SetBool("Falling", false);
    }
    // MASIH DI TANAH
    else if (grounded && isGround)
    {
        // do nothing
    }

   // DI UDARA
    else
    {
        isGround = false;
    }
}



void FixedUpdate()
{
    CheckGround();
}

}

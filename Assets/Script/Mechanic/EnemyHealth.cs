using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private Animator animator;
    private Collider2D col;
    private Rigidbody2D rb;

    private bool isDead = false;
    public bool IsDead => isDead;   // 🔥 THIS LINE FIXES THE ERROR

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= (int)damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (col != null)
            col.enabled = false;

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 1.2f); // match death animation length
    }
}

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Transform respawnPoint;

    private bool isDead = false;

    private Animator animator;
    private Rigidbody rb;
    private PlayerControll playerController;
    private Collider col;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerControll>();
        col = GetComponent<Collider>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // ▶ Death animation
        animator.SetBool("isDead", true);

        // ❌ Disable movement
        if (playerController != null)
            playerController.enabled = false;

        // ❌ Disable physics
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        // ❌ Disable collider
        if (col != null)
            col.enabled = false;
    }

    // 🔁 Animation Event
    public void Respawn()
    {
        // Move to respawn
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;

        // Reset health
        currentHealth = maxHealth;
        isDead = false;

        // Reset animation
        animator.SetBool("isDead", false);

        // Re-enable physics
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        // Re-enable collider
        if (col != null)
            col.enabled = true;

        // Re-enable control
        if (playerController != null)
            playerController.enabled = true;
    }
}
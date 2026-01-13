using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public int maxHealth = 5;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chaseSpeed = 4f;
    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    private bool facingLeft = true;

    void Update()
    {
        if (maxHealth <= 0)
        {
            Die();
            return;
        }

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        inRange = dist <= attackRange;

        if (inRange)
        {
            // ✅ FIX FLIP LOGIC (TYPO FIX)
            if (player.position.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            // 🔥 CHASE
            if (dist > retrieveDistance)
            {
                animator.SetBool("Attack0", false);
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    chaseSpeed * Time.deltaTime
                );
            }
            else
            {
                // 🔥 ATTACK MODE → STOP MOVEMENT TOTAL
                animator.SetBool("Attack0", true);
                return; // ⭐ INI KUNCI FIX UTAMA
            }
        }
        else
        {
            animator.SetBool("Attack0", false);

            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (!hit && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (!hit && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        }
    }

    // Dipanggil via Animation Event
    public void Attack()
    {
        if (attackPoint == null) return;

        Collider2D collInfo =
            Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collInfo != null)
        {
            PlayerControll pc = collInfo.GetComponent<PlayerControll>();
            if (pc != null)
            {
                pc.TakeDamage(3);

                 Camera.main.GetComponent<CameraShake>().Shake();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;
        maxHealth -= damage;
    }

    void Die()
    {
        Debug.Log(name + " Died");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (checkPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}

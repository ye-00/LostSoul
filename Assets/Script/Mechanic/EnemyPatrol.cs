using NUnit.Framework.Internal;
using Unity.VisualScripting;
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
        }


        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else {
            inRange = false;
        }

        if (inRange)
        {
            if (player.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x > transform.position.x && facingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack0", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack0", true);
                return;
            }
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (hit == false && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (hit == false && facingLeft == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        } 
    }

    public void Attack()
    {
       Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
       
       if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PlayerControll>() != null)
            {
                collInfo.gameObject.GetComponent<PlayerControll>().TakeDamage(1);
            }
        }
    } 

    public void TakeDamage(int damage)
    {
        if (maxHealth <=0)
        return;
        {
        maxHealth -= damage;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (checkPoint == null) return;
        {
            
        }
        
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
      
        if (attackPoint == null) return;
        Gizmos.color = Color.pink;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

    }

    void Die()
    {
        Debug.Log(this.transform.name + "Died");
        Destroy(this.gameObject);
    }
}
  
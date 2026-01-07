using UnityEngine;

public class WalkTest : MonoBehaviour
{
    public Transform entryPoint;
    public Transform spawnPoint;
    public float speed = 3f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = entryPoint.position;
    }

    void FixedUpdate()
    {
        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                spawnPoint.position,
                speed * Time.fixedDeltaTime
            )
        );
    }
}
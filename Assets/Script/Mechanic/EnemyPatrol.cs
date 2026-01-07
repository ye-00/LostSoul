using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    private bool facingLeft = true;

    void Update()
    {
        // 1️⃣ Gerak sesuai arah
        float dir = facingLeft ? -1f : 1f;
        transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);

        // 2️⃣ Raycast ke bawah (cek ground)
        RaycastHit2D hit = Physics2D.Raycast(
            checkPoint.position,
            Vector2.down,
            distance,
            layerMask
        );

        // 3️⃣ Kalau ga ada ground → balik arah
        if (!hit.collider)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (checkPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
    }
}

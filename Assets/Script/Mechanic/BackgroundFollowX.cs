using UnityEngine;

public class BackgroundFollowX : MonoBehaviour
{
    public Transform cam;
    public float parallaxEffect = 0.3f;
    public float pixelsPerUnit = 16f;

    private float startPos;
    private float length;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate() // IMPORTANT
    {
        float camX = cam.position.x;

        // parallax movement
        float distance = camX * parallaxEffect;
        float movement = camX * (1 - parallaxEffect);

        // PIXEL SNAP (THIS REMOVES JITTER)
        float snappedX = Mathf.Round((startPos + distance) * pixelsPerUnit) / pixelsPerUnit;

        transform.position = new Vector3(
            snappedX,
            transform.position.y,
            transform.position.z
        );

        // infinite background looping
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}

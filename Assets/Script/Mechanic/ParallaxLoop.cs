using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    public Transform cam;
    public float parallaxSpeed = 0.5f;

    float spriteWidth;
    Transform[] tiles;
    Vector3 lastCamPos;

    void Start()
    {
        lastCamPos = cam.position;

        tiles = new Transform[transform.childCount];
        for (int i = 0; i < tiles.Length; i++)
            tiles[i] = transform.GetChild(i);

        spriteWidth = tiles[0]
            .GetComponent<SpriteRenderer>()
            .bounds.size.x;
    }

    void LateUpdate()
    {
        // PARALLAX
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(delta.x * parallaxSpeed, 0, 0);
        lastCamPos = cam.position;

        // LOOPING
        foreach (Transform tile in tiles)
        {
            if (cam.position.x - tile.position.x > spriteWidth)
            {
                tile.position += Vector3.right * spriteWidth * tiles.Length;
            }
        }
    }
}

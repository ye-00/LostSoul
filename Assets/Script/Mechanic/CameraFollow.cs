using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    [Header("Locked Camera Position")]
    public float fixedY = 2f;
    public float fixedZ = -10f;

    private bool followEnabled = true;

    void LateUpdate()
    {
        if (!followEnabled || target == null)
            return;

        Vector3 desiredPosition = new Vector3(
            target.position.x,   // follow X only
            fixedY,              // LOCK Y
            fixedZ               // LOCK Z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void DisableFollow()
    {
        followEnabled = false;
    }

    public void EnableFollow()
    {
        // snap clean
        transform.position = new Vector3(
            target.position.x,
            fixedY,
            fixedZ
        );

        followEnabled = true;
    }
}
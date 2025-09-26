/*
    Ethan Gapic-Kott, 000923124
*/

/// Script used to tether the camera to the player gameobject

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    [HideInInspector]
    public bool isPlayerDashing = false;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;

        // Snaps camera to player gameobject
        transform.position = new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z);
    }

}

using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;      // Player capsule
    public float distance = 5f;   // Distance behind the player
    public float height = 3f;     // Height above the player
    public float mouseSensitivity = 2f;

    private float yaw = 0f;       // Horizontal rotation
    private float pitch = 20f;    // Vertical rotation

    void LateUpdate()
    {
        // --- Mouse Input ---
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -20f, 45f); // Limit vertical rotation

        // --- Calculate Camera Rotation ---
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // --- Set Camera Position ---
        Vector3 offset = rotation * new Vector3(0, 0, -distance); // Camera behind player
        transform.position = player.position + Vector3.up * height + offset;

        // --- Look at Player ---
        transform.LookAt(player.position + Vector3.up * 1f); // Look slightly above center
    }
}
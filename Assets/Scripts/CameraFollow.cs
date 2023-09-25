using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's Transform
    public Vector3 offset = new Vector3(0, 2, -10); // Offset from the player's position
    public float smoothSpeed = 5.0f; // Smoothing factor for camera movement
    
    private void LateUpdate()
    {
        if (target == null)
        {
            // Ensure you have a reference to the player GameObject
            Debug.LogWarning("Camera target not set!");
            return;
        }

        // Calculate the desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}

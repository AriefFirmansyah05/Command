using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's Transform
    public Vector3 offset = new Vector3(0, 2, -10); // Offset from the player's position
    public float smoothSpeed = 5.0f; // Smoothing factor for camera movement

    public Vector2 minBoundary; // Minimum camera position
    public Vector2 maxBoundary; // Maximum camera position

    private CharacterController2D characterController; // Reference to the CharacterController2D script
    private float initialYPosition; // Initial Y position of the camera

    private void Start()
    {
        // Find and store a reference to the CharacterController2D script
        characterController = target.GetComponent<CharacterController2D>();

        // Ensure you have a reference to the player GameObject
        if (target == null)
        {
            Debug.LogWarning("Camera target not set!");
            return;
        }

        // Store the initial Y position of the camera
        initialYPosition = transform.position.y;
    }

private void LateUpdate()
{
    if (characterController != null)
    {
        // Calculate the desired X position (follow player horizontally)
        float desiredX = target.position.x + offset.x;

        // Keep the initial Y position of the camera
        float desiredY = initialYPosition;

        // Calculate the desired camera position
        Vector3 desiredPosition = new Vector3(desiredX, desiredY, offset.z);

        // Ensure the camera stays within the boundary
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBoundary.x, maxBoundary.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBoundary.y, maxBoundary.y);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}

}

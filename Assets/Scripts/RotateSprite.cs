using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public float rotationSpeed = 90f; // Adjust the speed as needed

    void Update()
    {
        // Rotate the GameObject around the Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}

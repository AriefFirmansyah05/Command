using UnityEngine;
using TMPro;

public class DisableTextMeshPro : MonoBehaviour
{
    public TextMeshPro textComponent;
    public float disableDelay = 5.0f; // Adjust this to the desired delay in seconds
    public float animationDistance = 10f; // Adjust this to set the distance the text should move

    private Vector3 originalPosition;
    private void Start()
    {
        originalPosition = textComponent.transform.position; // Store the original position
        StartCoroutine(DisableTextDelayed());
    }

    private System.Collections.IEnumerator DisableTextDelayed()
    {
        // Move the text upwards
        Vector3 targetPosition = originalPosition + Vector3.up * animationDistance;
        float elapsedTime = 0f;

        while (elapsedTime < disableDelay)
        {
            textComponent.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / disableDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is at the target position when the animation ends
        textComponent.transform.position = targetPosition;

        yield return new WaitForSeconds(0.1f); // A small delay for the text to stay in its final position
        textComponent.enabled = false; // Disable the TextMeshPro Text component
    }
}

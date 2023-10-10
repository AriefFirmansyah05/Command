using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro Text element

    private void Start()
    {
        // Make sure you assign the TextMeshPro Text component in the Inspector to the "scoreText" variable.
        if (scoreText == null)
        {
            Debug.LogError("TextMeshPro Text component is not assigned to the ScoreDisplay script.");
        }
    }

    private void Update()
    {
        // Update the TextMeshPro Text component with the current score from the ScoreManager.
        scoreText.text = "Score: " + ScoreManager.instance.score.ToString();
    }
}

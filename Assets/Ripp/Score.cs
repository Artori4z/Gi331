using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int score;
    
    public TextMeshProUGUI scoreText;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            score += 1;
            scoreText.text = "Score: " + score;
        }
    }
}

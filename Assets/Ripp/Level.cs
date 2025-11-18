using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int levelNumber;
    
    public TextMeshProUGUI levelText;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            levelNumber += 1;
            levelText.text = "Level " + levelNumber;
        }
    }
}

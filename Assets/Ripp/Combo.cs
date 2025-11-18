using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int combo;
    
    public TextMeshProUGUI comboText;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            combo += 1;
            comboText.text = "X"+combo+"!!";
        }
    }
}

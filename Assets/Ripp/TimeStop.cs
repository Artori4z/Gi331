using UnityEngine;
using TMPro;

public class TimeStop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float time;
    
    public TextMeshProUGUI timeText;
    

    void Update()
    {
        time += Time.deltaTime;
        timeText.text = "Time: " + Mathf.RoundToInt(time);
    }
}

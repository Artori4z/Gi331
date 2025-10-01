using UnityEngine;

public class Cube : MonoBehaviour
{

    public float speedDown = 1f;
    GamePlayPao gamePlay;
    Rigidbody rb;
    public Transform startPoint;   // จุดเริ่มทางซ้าย
    public Transform midPoint;     // จุดต่ำสุด (กลาง)
    public Transform endPoint;     // จุดทางขวา
    public float speed = 1f;       // ความเร็วการเคลื่อนที่
    private float t = 0f;
    public float rayLength = 5f;
    [System.Obsolete]
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gamePlay = FindObjectOfType<GamePlayPao>();
        startPoint = GameObject.Find("Start").transform;
        midPoint = GameObject.Find("SpawnPoint").transform;
        endPoint = GameObject.Find("End").transform;
    }
    void Update()
    {
        if (gamePlay.isMoving)
        {
            transform.position -= new Vector3(0, Time.deltaTime * speedDown, 0);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.blue);
            // เพิ่มค่าเวลา (0 → 1)
            t += Time.deltaTime * speed;

            // ใช้ quadratic Bezier curve: B(t) = (1 - t)^2 * start + 2(1 - t)t * mid + t^2 * end
            Vector3 position =
                Mathf.Pow(1 - t, 2) * startPoint.position +
                2 * (1 - t) * t * midPoint.position +
                Mathf.Pow(t, 2) * endPoint.position;

            transform.position = position;

            // เมื่อถึงจุดสิ้นสุดให้วนกลับ
            if (t > 1f)
            {
                // กลับทิศ (ไปกลับ)
                (startPoint, endPoint) = (endPoint, startPoint);
                t = 0f;
            }
        } 
    }

    
        
        
    private void OnCollisionEnter(Collision collision)
    {
        // ถ้า tag ของวัตถุที่ชนคือ "Cube"
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            Destroy(rb);
            Destroy(this);
        }
        // ถ้า tag ของวัตถุที่ชนคือ "Lose"
        else if (collision.gameObject.CompareTag("Lose"))
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            Destroy(this);
        }
    }
}


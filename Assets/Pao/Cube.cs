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
    public float timeDelay = 1f;
    public bool goingToStart = true;
    public bool hit = false;
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
            //transform.position -= new Vector3(0, Time.deltaTime * speedDown, 0);
        }
        else if(!hit)
        {
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
        if (hit)
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            timeDelay += Time.deltaTime;
            if (timeDelay >= 3)
            {
                gamePlay.hascube = false;
                gamePlay.Cam.transform.position += new Vector3(0, gamePlay.cubeHeight, 0);
                Destroy(rb);
                Destroy(this);
                hit = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // ถ้า tag ของวัตถุที่ชนคือ "Cube"
        if (collision.gameObject.CompareTag("Cube"))
        {
            hit = true;
            if (collision.transform.position.x == this.transform.position.x)
            {
                Debug.Log("Perfect");
            }
        }
        // ถ้า tag ของวัตถุที่ชนคือ "Lose"
        else if (collision.gameObject.CompareTag("Lose"))
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            Destroy(this.gameObject);
            gamePlay.life -= 1;
            Debug.Log(gamePlay.life);
            gamePlay.hascube = false;
        }
    }
}


using UnityEngine;

public class Cube : MonoBehaviour
{

    public float speedDown = Physics.gravity.y;
    public GamePlayPao gamePlay;
    Rigidbody rb;
    public Transform startPoint;   // จุดเริ่มทางซ้าย
    public Transform midPoint;     // จุดต่ำสุด (กลาง)
    public Transform endPoint;     // จุดทางขวา
    public float speed = 1f;       // ความเร็วการเคลื่อนที่
    private float t = 0f;
    public float timeDelay = 0f;
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

        if (gamePlay.isMoving == true)
        {
            rb.useGravity = true;
            rb.AddForce(Physics.gravity * 0.5f, ForceMode.Acceleration);
        }
        if (!hit && gamePlay.isMoving == false)
        {
            t += Time.deltaTime * speed;
            rb.useGravity = false;
            Vector3 position =
                Mathf.Pow(1 - t, 2) * startPoint.position +
                2 * (1 - t) * t * midPoint.position +
                Mathf.Pow(t, 2) * endPoint.position;

            transform.position = position;

            if (t > 1f)
            {
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

            float zRot = transform.eulerAngles.z;
            // แปลงให้ช่วงอยู่ระหว่าง -180 ถึง 180 (จะได้ไม่ต้องกังวลค่า 359)
            if (zRot > 180f) zRot -= 360f;

            if (Mathf.Abs(zRot) <= 1f)
            {
                timeDelay += Time.deltaTime;

                if (timeDelay >= 2f)
                {
                    gamePlay.hascube = false;
                    gamePlay.Cam.transform.position += new Vector3(0, gamePlay.cubeHeight, 0);
                    Destroy(rb);
                    Destroy(this);
                    hit = false;
                    gamePlay.cubecount++;
                }
            }
            else
            {
                timeDelay = 0f;
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


using UnityEngine;

public class Cube : MonoBehaviour
{
    public GamePlayPao gamePlay;
    Rigidbody rb;
    public Transform startPoint;
    public Transform midPoint;
    public Transform endPoint;
    private float t = 0f;
    public float timeDelay = 0f;
    public bool goingToStart = true;
    public bool hit = false;
    private bool initialized = false;
    public float speed = 2;
    private enum StartType { Start, Mid, End }
    private StartType startType;
    private int phase = 0;
    Vector3 position;
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
        switch (gamePlay.Lv)
        {
            case 1:
                speed = 1f;
                break;
            case 2:
                speed = 1.5f;
                break;
            case 3:
                speed = 2f;
                break;
        }
        if (gamePlay.isMoving)
        {
            if (gamePlay.boots)
            {
                rb.useGravity = true;
                rb.AddForce(Physics.gravity * 100, ForceMode.Acceleration);
            }
            rb.useGravity = true;
            rb.AddForce(Physics.gravity * 10, ForceMode.Acceleration);
        }
        if (!gamePlay.isMoving)
        {
            rb.useGravity = false;
        }
        if (!hit && !gamePlay.isMoving && !gamePlay.boots)
        {
            if (!initialized)
            {
                if (gamePlay.Lv > 3)
                {
                    speed = gamePlay.EndlessSpeed;
                }
                int randomStart = gamePlay.randomSpawn;
                switch (randomStart)
                {
                    case 1:
                        startType = StartType.Start;
                        transform.position = startPoint.position;
                        phase = 0;
                        break;
                    case 0:
                        startType = StartType.Mid;
                        transform.position = midPoint.position;
                        phase = 0;
                        break;
                    case 2:
                        startType = StartType.End;
                        transform.position = endPoint.position;
                        phase = 0;
                        break;
                }
                initialized = true;
            }
            switch (startType)
            {
                case StartType.Start:
                    t += Time.deltaTime * speed;
                    rb.useGravity = false; 
                    position = Mathf.Pow(1 - t, 2) 
                        * startPoint.position + 2 * (1 - t) * t 
                        * midPoint.position + Mathf.Pow(t, 2) 
                        * endPoint.position;
                    transform.position = position; 
                    if (t > 1f)
                    {
                        (startPoint, endPoint) = (endPoint, startPoint);
                        t = 0f; 
                    }
                    break;
                case StartType.Mid:
                    if (phase == 0)
                    {
                        t += Time.deltaTime * (speed*2);
                        Vector3 position =
                            Mathf.Pow(1 - t, 2) * midPoint.position +
                            2 * (1 - t) * t * ((midPoint.position + endPoint.position) / 2) +
                            Mathf.Pow(t, 2) * endPoint.position;
                        transform.position = position;
                        if (t >= 1f) { t = 0f; phase = 1; }
                    }
                    else if (phase == 1)
                    {
                        t += Time.deltaTime * speed;
                        rb.useGravity = false;
                        position = Mathf.Pow(1 - t, 2)
                            * endPoint.position + 2 * (1 - t) * t
                            * midPoint.position + Mathf.Pow(t, 2)
                            * startPoint.position;
                        transform.position = position;
                        if (t > 1f)
                        {
                            (startPoint, endPoint) = (endPoint, startPoint);
                            t = 0f;
                        }
                    }
                    break;
                case StartType.End:
                    t += Time.deltaTime * speed;
                    rb.useGravity = false;
                    position = Mathf.Pow(1 - t, 2)
                        * endPoint.position + 2 * (1 - t) * t
                        * midPoint.position + Mathf.Pow(t, 2)
                        * startPoint.position;
                    transform.position = position;
                    if (t > 1f)
                    {
                        (startPoint, endPoint) = (endPoint, startPoint);
                        t = 0f;
                    }
                    break;
            }
        }
        if (hit)
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            if (!gamePlay.boots)
            {
                if (gamePlay.perfectCount == 9)
                {
                    timeDelay += Time.deltaTime;
                    if (timeDelay >= 1f)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 0);
                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                        if (this.transform.position == new Vector3(0, this.transform.position.y, this.transform.position.z))
                        {
                            gamePlay.hascube = false;
                            gamePlay.Cam.transform.position += new Vector3(0, gamePlay.cubeHeight, 0);
                            Destroy(rb);
                            Destroy(this);
                            hit = false;
                            gamePlay.cubecount++;
                            gamePlay.perfectCount++;
                            if (gamePlay.Lv > 3)
                            {
                                gamePlay.EndlessSpeed += 0.1f;
                            }
                        }
                    }
                }
                else
                {
                    float zRot = transform.eulerAngles.z;
                    if (zRot > 180f) zRot -= 360f;
                    timeDelay += Time.deltaTime;
                    if (Mathf.Abs(zRot) <= 10f)
                    {
                        if (timeDelay >= 1f)
                        {
                            gamePlay.hascube = false;
                            gamePlay.Cam.transform.position += new Vector3(0, gamePlay.cubeHeight, 0);
                            Destroy(rb);
                            Destroy(this);
                            hit = false;
                            gamePlay.cubecount++;
                            gamePlay.perfectCount++;
                            if (gamePlay.Lv > 3)
                            {
                                gamePlay.EndlessSpeed += 0.1f;
                            }
                        }
                    }
                }
            }
            else if (gamePlay.boots)
            {
                gamePlay.hascube = false;
                gamePlay.Cam.transform.position += new Vector3(0, gamePlay.cubeHeight, 0);
                Destroy(rb);
                Destroy(this);
                hit = false;
                gamePlay.cubecount++;
                rb.useGravity = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (gamePlay.Lv2Swap)
            {
                Debug.Log("true");
                gamePlay.Lv2Swap = false;
            }else
            {
                Debug.Log("false");
                gamePlay.Lv2Swap = true;
            }
            hit = true;
        }
        else if (collision.gameObject.CompareTag("Lose"))
        {
            if (gamePlay != null)
            {
                gamePlay.isMoving = false;
            }
            Destroy(this.gameObject);
            gamePlay.life -= 1;
            gamePlay.hascube = false;
            gamePlay.perfectCount = 0;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            hit = false;
            gamePlay.isMoving = true;
            timeDelay = 0;
        }
    }
}
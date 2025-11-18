using UnityEngine;

public class building : MonoBehaviour
{
    //GamePlay & Transform
    public GamePlay gamePlay;
    Rigidbody rb;
    public Transform startPoint;
    public Transform midPoint;
    public Transform endPoint;
    //ขยับซ้าย ขวา 
    private float t = 0f;
    public float speed = 2;
    public bool goingToStart = true;
    private bool initialized = false;
    private enum StartType { Start, Mid, End }
    private StartType startType;
    private int phase = 0;
    Vector3 position;
    //ดูตอนตึกโดนตึก
    public float timeDelay = 0f;
    public bool hit = false;
    [System.Obsolete]
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gamePlay = FindObjectOfType<GamePlay>();
        startPoint = GameObject.Find("Start").transform;
        midPoint = GameObject.Find("SpawnPoint").transform;
        endPoint = GameObject.Find("End").transform;
    }
    void Update()
    {
        //ปรับความเร็วตามLv
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
        //กดจอแล้วตึกตก
        if (gamePlay.IsMoving)
        {
            //ของตอนมีBoots
            if (gamePlay.boots)
            {
                rb.useGravity = true;
                rb.AddForce(Physics.gravity * 100, ForceMode.Acceleration);
            }
            //ของตอนปกติ
            rb.useGravity = true;
            rb.AddForce(Physics.gravity * 10, ForceMode.Acceleration);
        }
        else
        {
            //ตอนยังไม่กด
            rb.useGravity = false;
        }
        //ขยับตอนยังไม่ได้กด
        if (!hit && !gamePlay.IsMoving && !gamePlay.boots)
        {
            //ดูว่าสร้างตึกที่Pointไหน
            if (!initialized)
            {
                //เข้าEndless
                if (gamePlay.Lv > 3)
                {
                    speed = gamePlay.EndlessSpeed;
                }
                int randomStart = gamePlay.RandomSpawn;
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
            //ขยับซ้ายขวา
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
                        t += Time.deltaTime * (speed * 2);
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
        //ตึกโดนตึกก่อนหน้า
        if (hit)
        {
            if (gamePlay != null)
            {
                gamePlay.IsMoving = false;
            }
            if (!gamePlay.boots)
            {
                //ทำให้ตึกก่อนเข้าbootsมาอยุ่ตรงกลาง
                if (gamePlay.PerfectCount == 9)
                {
                    timeDelay += Time.deltaTime;
                    if (timeDelay >= 1f)
                    {
                        this.transform.rotation = Quaternion.Euler(0, 0, 0);
                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                        if (this.transform.position == new Vector3(0, this.transform.position.y, this.transform.position.z))
                        {
                            gamePlay.HasBuilding = false;
                            gamePlay.Cam.transform.position += new Vector3(0, gamePlay.BuildingHeight, 0);
                            Destroy(rb);
                            Destroy(this);
                            hit = false;
                            gamePlay.BuildingCount++;
                            gamePlay.PerfectCount++;
                        }
                    }
                }
                else
                {
                    //วัดความเอียงของตึก
                    float zRotation = transform.eulerAngles.z;
                    if (zRotation > 180f) zRotation -= 360f;
                    timeDelay += Time.deltaTime;
                    //กันไม่ให้ตึกเอียงไปมานานเกิน
                    if (timeDelay >= 5f)
                    {
                        //ทำให้ตึกอยุ่กับที่
                        gamePlay.HasBuilding = false;
                        gamePlay.Cam.transform.position += new Vector3(0, gamePlay.BuildingHeight, 0);
                        Destroy(rb);
                        Destroy(this);
                        hit = false;
                        gamePlay.BuildingCount++;
                        gamePlay.PerfectCount++;
                        //+ความเร็วถ้าเข้าEndless
                        if (gamePlay.Lv > 3)
                        {
                            gamePlay.EndlessSpeed += 0.1f;
                        }
                    }
                    if (Mathf.Abs(zRotation) <= 5f)
                    {
                        
                        if (timeDelay >= 1f)
                        {
                            //ทำให้ตึกอยุ่กับที่
                            gamePlay.HasBuilding = false;
                            gamePlay.Cam.transform.position += new Vector3(0, gamePlay.BuildingHeight, 0);
                            Destroy(rb);
                            Destroy(this);
                            hit = false;
                            gamePlay.BuildingCount++;
                            gamePlay.PerfectCount++;
                            //+ความเร็วถ้าเข้าEndless
                            if (gamePlay.Lv > 3)
                            {
                                gamePlay.EndlessSpeed += 0.1f;
                            }
                        }
                    }
                }
            }
            //ของตอนมีBoots
            else if (gamePlay.boots)
            {
                gamePlay.HasBuilding = false;
                gamePlay.Cam.transform.position += new Vector3(0, gamePlay.BuildingHeight, 0);
                Destroy(rb);
                Destroy(this);
                hit = false;
                gamePlay.BuildingCount++;
                rb.useGravity = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //โดนตึกก่อนหน้า
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (gamePlay.Lv2Swap)
            {
                Debug.Log("true");
                gamePlay.Lv2Swap = false;
            }
            else
            {
                Debug.Log("false");
                gamePlay.Lv2Swap = true;
            }
            hit = true;
        }
        //โดนอันที่ตั้งไว้ว่าแพ้
        else if (collision.gameObject.CompareTag("Lose"))
        {
            if (gamePlay != null)
            {
                gamePlay.IsMoving = false;
            }
            Destroy(this.gameObject);
            gamePlay.Life -= 1;
            gamePlay.HasBuilding = false;
            gamePlay.PerfectCount = 0;
        }
    }
    //ดูว่าตึกยังโดนอีกตึกอยุ่มั้ย
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            hit = false;
            gamePlay.IsMoving = true;
            timeDelay = 0;
        }
    }
}
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayPao : MonoBehaviour
{
    [SerializeField] private GameObject[] Cube;
    [SerializeField] private GameObject[] SpawnPoint;
    [SerializeField] public GameObject Cam;
    [SerializeField] public GameObject[] LvTwo;
    public int cubecount = 0;
    public float cubeHeight;
    public bool isMoving = false;
    public bool hascube = false;
    public float canSpawm = 5;
    int random;
    public int life;
    public int randomSpawn = 0;
    public int Lv = 3;
    public int perfectCount = 0;
    public float EndlessSpeed = 2;
    public bool Lv2Swap;
    private bool moveUpOne = true;
    private bool moveUpTwo = true;
    private float fixedZ;
    public float bootsTimer = 0f;
    public bool boots;
    public Material[] Sky;
    private void Start()
     {
        Instantiate(Cube[0], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
        cubeHeight = Cube[0].transform.localScale.y;
        hascube = true;
        isMoving = false;
        life = 3;
        boots = false;
        fixedZ = SpawnPoint[1].transform.localPosition.z;
        RenderSettings.skybox = Sky[0];
        DynamicGI.UpdateEnvironment();
    }
    void Update()
    {
        if (perfectCount == 10)
        {
            boots = true;
        }
        if (bootsTimer >= 5)
        {
            boots = false;
            perfectCount = 0;
            bootsTimer = 0;
        }
        if (boots)
        {
             bootsTimer += Time.deltaTime;
            if (!hascube)
                {
                    Instantiate(Cube[0], SpawnPoint[0].transform.position, Quaternion.identity);
                    cubeHeight = Cube[random].transform.localScale.y;
                    hascube = true;
                    isMoving = false;  
                }
        } 
        else
        {
            if (!hascube)
            {
                    random = Random.Range(0, Cube.Length);
                    randomSpawn = Random.Range(0, SpawnPoint.Length);
                    cubeHeight = Cube[random].transform.localScale.y;
                    Instantiate(Cube[random], SpawnPoint[randomSpawn].transform.position, Quaternion.identity);
                    hascube = true;
                    isMoving = false;
            }
        }
        if ((Keyboard.current.spaceKey.isPressed && hascube) || (Input.touchCount > 0 && hascube))
        {
            isMoving = true;
        }
        switch (cubecount)
        {
            case 10:
                Lv = 2;
                break;
            case 15:
                Lv = 3;
                break;
            case 20:
                Lv = 4;
                break;
        }
        if (Lv == 2)
        {
            RenderSettings.skybox = Sky[1];
            DynamicGI.UpdateEnvironment();
            if (Lv2Swap)
            {
                SpawnPoint[0].transform.position = LvTwo[0].transform.position;
                SpawnPoint[1].transform.position = LvTwo[3].transform.position;
                SpawnPoint[2].transform.position = LvTwo[4].transform.position;
            }
            else
            {
                SpawnPoint[0].transform.position = LvTwo[0].transform.position;
                SpawnPoint[1].transform.position = LvTwo[1].transform.position;
                SpawnPoint[2].transform.position = LvTwo[2].transform.position;
            }
        }else if (Lv >= 3)
        {
            RenderSettings.skybox = Sky[2];
            DynamicGI.UpdateEnvironment();
            MoveSpawnPointUpDown(SpawnPoint[1], ref moveUpOne, fixedZ, LvTwo[1], LvTwo[3]);
            MoveSpawnPointUpDown(SpawnPoint[2], ref moveUpTwo, fixedZ, LvTwo[4], LvTwo[2]);          
        }
        if (life <= 0)
        {
            Time.timeScale = 0f;
        }
    }
    private void MoveSpawnPointUpDown(GameObject spawn, ref bool moveUp, float fixedLocalZ, GameObject minPoint, GameObject maxPoint)
    {
        Transform sp = spawn.transform;
        Vector3 localPos = sp.localPosition;
        if (localPos.y >= maxPoint.transform.localPosition.y)
            moveUp = false;
        else if (localPos.y <= minPoint.transform.localPosition.y)
            moveUp = true;
        float direction = moveUp ? 1f : -1f;
        localPos.y += direction * 2f * Time.deltaTime;
        localPos.z = fixedLocalZ;
        sp.localPosition = localPos;
    }
}

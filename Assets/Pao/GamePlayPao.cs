using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayPao : MonoBehaviour
{
    [SerializeField] private GameObject[] Cube;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] public GameObject Cam;
    public int cubecount = 0;
    public float cubeHeight;
    public bool isMoving = false;
    public bool hascube = false;
    public float time = 5;
    public float canSpawm = 5;
    int random;
    public int life;
     private void Start()
     {
        Instantiate(Cube[0], SpawnPoint.transform.position, Quaternion.identity);
        cubeHeight = Cube[0].transform.localScale.y;
        hascube = true;
        isMoving = false;
        life = 3;
        Debug.Log(life);
    }
    void Update()
    {
        if (!hascube)
        {
            random = Random.Range(0, Cube.Length);
            cubeHeight = Cube[random].transform.localScale.y;
            Instantiate(Cube[random], SpawnPoint.transform.position, Quaternion.identity);
            hascube = true;
            isMoving = false;
            time = 0;
        }

        if ((Input.GetMouseButtonDown(0) && hascube) || (Input.touchCount > 0 && hascube))
        {
            isMoving = true;
        }

        if(cubecount >= 10)
        {
            Debug.Log("win");
        }
        if(life <= 0)
        {
            Debug.Log("lose");
        }
    }
}

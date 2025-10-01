using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayPao : MonoBehaviour
{
    [SerializeField] private GameObject Cube;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private GameObject Cam;
    float cubeHeight;
    public bool isMoving = false;
    bool hascube = false;
    public float time = 5;
    public float canSpawm = 5;
    // Update is called once per frame
    private void Start()
    {
        cubeHeight = Cube.transform.localScale.y;
    }
    void Update()
    {
        if(!hascube)
        {
            time += Time.deltaTime;
        }
        
        if (!hascube && time >= canSpawm)
        {
            Instantiate(Cube, SpawnPoint.transform.position, Quaternion.identity);
            hascube = true;
            isMoving = false;
            time = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && hascube)
        {
            Cam.transform.position += new Vector3(0, cubeHeight, 0);
            isMoving = true;
            hascube = false;
        }
    }
}

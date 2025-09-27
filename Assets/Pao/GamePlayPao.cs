using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayPao : MonoBehaviour
{
    [SerializeField] private GameObject Cube;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private GameObject Cam;
    float cubeHeight;
    public bool isMoving = false;
    // Update is called once per frame
    private void Start()
    {
        cubeHeight = Cube.transform.localScale.y;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(Cube, SpawnPoint.transform.position, Quaternion.identity);
            Cam.transform.position += new Vector3(0, cubeHeight, 0);
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isMoving = true;
        }

    }
}

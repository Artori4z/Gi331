using UnityEngine;

public class Cube : MonoBehaviour
{

    public float speed = 1f;
    GamePlayPao gamePlay;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gamePlay = FindObjectOfType<GamePlayPao>();
    }
    void Update()
    {

        if (gamePlay.isMoving)
        {
            transform.position -= new Vector3(0, Time.deltaTime *9.81f, 0);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(rb);
        Destroy(this);
        if (gamePlay != null)
        {
            gamePlay.isMoving = false;
        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;

public class Destory : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube") &&
            collision.gameObject.name != "base")
        {
            Destroy(collision.gameObject);
        }
    }
}

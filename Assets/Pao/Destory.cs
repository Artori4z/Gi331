using UnityEngine;
using UnityEngine.InputSystem;

public class Destory : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ถ้า tag ของวัตถุที่ชนคือ "Cube"
        if (collision.gameObject.CompareTag("Cube"))
        {
            Debug.Log("hit");
            Destroy(collision.gameObject);
        }
    }
}

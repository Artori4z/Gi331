using UnityEngine;
using UnityEngine.InputSystem;

public class Reset : MonoBehaviour
{
    public UIManager manager;
    public GamePlay gamePlay;

    [System.Obsolete]
    private void Start()
    {
        manager = FindObjectOfType<UIManager>();
        gamePlay = FindObjectOfType<GamePlay>();
    }
    // Update is called once per frame
    void Update()
    {
        if(manager.Reset)
        {
            Destroy(gameObject);
            if(gamePlay.HasBuilding || gamePlay.IsMoving)
            {
                gamePlay.HasBuilding = false;
                gamePlay.IsMoving = false;
            }
        }
    }
}

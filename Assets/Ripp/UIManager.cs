using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;

    public GamePlay gameplay;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
        
        Time.timeScale = 0f;
        gameplay.enabled = false;
    }

    public void SetOnTimeScale()
    {
        Time.timeScale = 1f;
    }

    public void setOffTimeScale()
    {
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

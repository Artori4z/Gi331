using TMPro;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject levelSelect;
    public GamePlay gameplay;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI scoreTextEnd;
    public TextMeshProUGUI scoreTextPause;
    public bool Reset;
    void Start()
    {
        MainMenu();
        Reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameplay.BuildingCount;
        lifeText.text = "Life: " + gameplay.Life;
        scoreTextEnd.text = "Score: " + gameplay.BuildingCount;
        scoreTextPause.text = "Score: " + gameplay.BuildingCount;
        if (gameplay.Lv > 3)
        {
            levelText.text = "Endless";
        }
        else
        {
            levelText.text = "Level " + gameplay.Lv;
        }
        if(gameplay.PerfectCount > 0)
        {
            comboText.text = "X" + gameplay.PerfectCount + "!!";
        }
        else
        {
            comboText.text = "";
        }
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
        gameplay.enabled = true;
    }

    public void setOffTimeScale()
    {
        Time.timeScale = 0f;
        gameplay.enabled = false;
        gameplay.IsMoving = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetLvOne()
    {
        gameplay.Reset();
        gameplay.Lv = 1;
    }
    public void SetLvTwo()
    {
        gameplay.Reset();
        gameplay.Lv = 2;
    }
    public void SetLvThree()
    {
        gameplay.Reset();
        gameplay.Lv = 3;
    }
    public void SceneReset()
    {
        Reset = true;
        gameplay.Lv = 1;
        gameplay.Life = 3;
        gameplay.BuildingCount = 0;
        gameplay.Cam.transform.position = new Vector3(0, 61, -110);
        gameplay.JustReset = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void UnSceneReset()
    {
        Reset = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

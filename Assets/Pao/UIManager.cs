using TMPro;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;
    public GameObject[] IconMute;
    public GamePlay gameplay;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI scoreTextEnd;
    public TextMeshProUGUI scoreTextPause;
    public bool Reset;
    public bool Mute;
    public bool StartLv;
    public float TimePerLv;
    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip uiOpenClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip resumeClip;

    async void Start()
    {
        MainMenu();
        Reset = false;
        Mute = false;
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics Ready!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Init Error: " + e.Message);
        }
    }
    private void FixedUpdate()
    {
        TimePerLv += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F))
        {
            CustomEvent buyEvent = new CustomEvent("DobedoEvent")
            {
                { "TimePerLv", TimePerLv }
            };
            AnalyticsService.Instance.RecordEvent(buyEvent);
            Debug.Log("time");
        }
    }
    void Update()
    {
        scoreText.text = "Score: " + gameplay.BuildingCount;
        lifeText.text = "Life: " + gameplay.Life;
        scoreTextPause.text = "Score: " + gameplay.BuildingCount;
        scoreTextEnd.text = "Your High Score: " + gameplay.highScore + "!!";

        if (gameplay.Lv > 3)
        {
            levelText.text = "Endless";
        }
        else
        {
            levelText.text = "Level " + gameplay.Lv;
        }

        if (gameplay.PerfectCount > 0)
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

        PlayUI(uiOpenClip);
    }

    public void SetOnTimeScale()
    {
        Time.timeScale = 1f;
        gameplay.enabled = true;

        if (resumeClip != null)
            PlayUI(resumeClip);
        else
            PlayUI(buttonClickClip);
    }

    public void setOffTimeScale()
    {
        Time.timeScale = 0f;
        gameplay.enabled = false;
        gameplay.IsMoving = false;
        TimePerLv = 0;
        if (pauseClip != null)
            PlayUI(pauseClip);
        else
            PlayUI(buttonClickClip);
    }

    public void QuitGame()
    {
        PlayUI(buttonClickClip);
        Application.Quit();
    }

    public void SetLvOne()
    {
        PlayUI(buttonClickClip);
        gameplay.Reset();
        gameplay.Lv = 1;
        CustomEvent buyEvent = new CustomEvent("DobedoEvent")
            {
                { "FavLevel", 1 }
            };
        AnalyticsService.Instance.RecordEvent(buyEvent);
    }

    public void SetLvTwo()
    {
        PlayUI(buttonClickClip);
        gameplay.Reset();
        gameplay.Lv = 2;
        CustomEvent buyEvent = new CustomEvent("DobedoEvent")
            {
                { "FavLevel", 2 }
            };
        AnalyticsService.Instance.RecordEvent(buyEvent);
    }

    public void SetLvThree()
    {
        PlayUI(buttonClickClip);
        gameplay.Reset();
        gameplay.Lv = 3;
        CustomEvent buyEvent = new CustomEvent("DobedoEvent")
            {
                { "FavLevel", 3 }
            };
        AnalyticsService.Instance.RecordEvent(buyEvent);
    }

    public void SceneReset()
    {
        Reset = true;
        gameplay.Lv = 1;
        gameplay.Life = 3;
        gameplay.BuildingCount = 0;
        gameplay.boots = false;
        gameplay.PerfectCount = 0;
        gameplay.bootsTimer = 0;
        gameplay.Cam.transform.position = new Vector3(0, 61, -110);
        gameplay.JustReset = true;
        
        StartLv = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UnSceneReset()
    {
        
        Reset = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Sound()
    {
        if (Mute)
        {
            AudioListener.volume = 1f;
            Mute = false;
            IconMute[0].SetActive(false);
            IconMute[1].SetActive(false);
            IconMute[2].SetActive(false);
        }
        else
        {
            AudioListener.volume = 0f;
            Mute = true;
            IconMute[0].SetActive(true);
            IconMute[1].SetActive(true);
            IconMute[2].SetActive(true);
        }
    }
    private void PlayUI(AudioClip clip)
    {
        if (clip != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(clip);
        }
    }
}

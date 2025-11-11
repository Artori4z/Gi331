using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayPao : MonoBehaviour
{   
    //stuctor
    [SerializeField] private GameObject[] Cube;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] public GameObject Cam;
    int cubecount = 0;
    public float cubeHeight;
    public bool isMoving = false;
    public bool hascube = false;
    public float time = 5;
    public float canSpawm = 5;
    int random;
    public int life;

    [Header("=== Audio Sources ===")]
    public AudioSource SFXSource;
    public AudioSource MusicSource;

    [Header("=== SFX Clips ===")]
    public AudioClip sfxSpawn;
    public AudioClip sfxImpact;
    public AudioClip sfxLifeLost;
    public AudioClip sfxWin;
    public AudioClip sfxLose;

    [Header("=== BGM ===")]
    public AudioClip bgmLoop;

    //=========================

    private void Start()
    {
        if (MusicSource && bgmLoop)
        {
            MusicSource.clip = bgmLoop;
            MusicSource.loop = true;
            MusicSource.Play();
        }

        Instantiate(Cube[0], SpawnPoint.transform.position, Quaternion.identity);
        cubeHeight = Cube[0].transform.localScale.y;
        hascube = true;
        isMoving = false;
        life = 3;
        Debug.Log(life);

        PlaySFX(sfxSpawn);
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

            PlaySFX(sfxSpawn);
        }

        if ((Input.GetMouseButtonDown(0) && hascube) || (Input.touchCount > 0 && hascube))
        {
            isMoving = true;
            cubecount++;
            //fall sound 
        }

        if (cubecount >= 10)
        {
            Debug.Log("win");
            PlaySFX(sfxWin);
        }
        if (life <= 0)
        {
            Debug.Log("lose");
            PlaySFX(sfxLose);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(clip, volume);
        }
    }

    public void OnCubeImpact(bool isPerfect)
    {
        if (SFXSource && sfxImpact)
            PlaySFX(sfxImpact);
    }

    public void OnLifeLost()
    {
        if (SFXSource && sfxLifeLost)
            PlaySFX(sfxLifeLost);
    }
}

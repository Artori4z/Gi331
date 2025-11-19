using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlay : MonoBehaviour
{
    //GameObject
    [SerializeField] GameObject[] Cube;
    [SerializeField] GameObject[] SpawnPoint;
    [SerializeField] public GameObject Cam;
    [SerializeField] GameObject[] LvTwo;
    public UIManager manager;
    public GameObject YouLose;

    //เกี่ยวกับกล้อง
    public float BuildingHeight;
    int random;

    //เกี่ยวกับสร้างตัวตึก
    public int highScore;
    public int BuildingCount = 0;
    public bool IsMoving = false;
    public bool HasBuilding = false;
    public bool JustReset;
    public int RandomSpawn = 0;

    //Hp กับ Boots
    public int Life;
    public int PerfectCount = 0;
    public float bootsTimer = 0f;
    public bool boots;

    //Lv กับ Endless
    public int Lv = 1;
    public float EndlessSpeed = 2;
    public bool Lv2Swap;
    bool moveUpOne = true;
    bool moveUpTwo = true;
    float fixedZ;

    //Bg
    [SerializeField] Material[] Sky;
    int currentSkyLv = 1;

    // ------------------ AUDIO เพิ่มมา ------------------
    [Header("Audio - SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip blockSpawnClip;
    [SerializeField] private AudioClip bootsStartClip;
    [SerializeField] private AudioClip bootsEndClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField] private AudioClip envTransitionClip;

    [Header("Audio - BGM")]
    [SerializeField] private AudioClip bgmLv1Clip;
    [SerializeField] private AudioClip bgmLv2Clip;
    [SerializeField] private AudioClip bgmLv3Clip;

    int previousLv;
    // ---------------------------------------------------

    private void Start()
    {
        Life = 3;
        boots = false;
        fixedZ = SpawnPoint[1].transform.localPosition.z;
        RenderSettings.skybox = Sky[0];
        DynamicGI.UpdateEnvironment();

        previousLv = Lv;
        PlayBGMForLevel(Lv);   // เล่น BGM ตามเลเวลเริ่มต้น
    }

    void Update()
    {
        //นับว่าวางตึกติดกันโดยที่ไม่ตก → เปิด Boots เมื่อครบ 10
        if (PerfectCount == 10 && !boots)
        {
            boots = true;
            PlaySFX(bootsStartClip);
        }

        //ดูว่า Boots หมดหรือยัง
        if (bootsTimer >= 5)
        {
            boots = false;
            PerfectCount = 0;
            bootsTimer = 0;
            PlaySFX(bootsEndClip);
        }

        //สร้างตึกตอน Boots
        if (!JustReset && boots && !manager.Reset)
        {
            bootsTimer += Time.deltaTime;
            if (!HasBuilding) 
            {
                Instantiate(Cube[0], SpawnPoint[0].transform.position, Quaternion.identity);
                BuildingHeight = Cube[random].transform.localScale.y;
                HasBuilding = true;
                IsMoving = false;

                PlaySFX(blockSpawnClip);
            }
        }
        //สร้างตึกปกติ
        else if (!JustReset && !boots && !manager.Reset)
        {
            if (!HasBuilding)
            {
                random = Random.Range(0, Cube.Length);
                RandomSpawn = Random.Range(0, SpawnPoint.Length);
                BuildingHeight = Cube[random].transform.localScale.y;
                Instantiate(Cube[random], SpawnPoint[RandomSpawn].transform.position, Quaternion.identity);
                HasBuilding = true;
                IsMoving = false;

                PlaySFX(blockSpawnClip);
            }
        }
        //สร้างตึกหลัง Reset
        else if (JustReset && !manager.Reset)
        {
            bootsTimer += Time.deltaTime;
            if (!HasBuilding)
            {
                Instantiate(Cube[0], SpawnPoint[0].transform.position, Quaternion.identity);
                BuildingHeight = Cube[random].transform.localScale.y;
                HasBuilding = true;
                IsMoving = false;
                JustReset = false;
                RandomSpawn = 0;

                PlaySFX(blockSpawnClip);
            }
        }

        //กดจอวางตึก (เรื่องตก/แรง นับใน building.cs แล้ว)
        if (Input.touchCount > 0 && HasBuilding)
        {
            IsMoving = true;
        }

        //เปลี่ยน Lv ตามจำนวนตึก
        switch (BuildingCount)
        {
            case 10:
                Lv = 2;
                break;
            case 20:
                Lv = 3;
                break;
            case 30:
                Lv = 4;
                break;
        }

        //ถ้าเปลี่ยนเลเวล ให้เล่นเสียง + เปลี่ยนเพลง
        if (Lv != previousLv)
        {
            PlaySFX(levelUpClip);
            PlayBGMForLevel(Lv);
            previousLv = Lv;
        }

        //เปลี่ยน Bg
        if (Lv == 1 && currentSkyLv != 1)
        {
            if (currentSkyLv == 2)
            {
                currentSkyLv = 1;
                PlaySFX(envTransitionClip);
                StartCoroutine(FadeSkybox(Sky[1], Sky[0], 2f));
            }
            else if (currentSkyLv >= 3)
            {
                currentSkyLv = 1;
                PlaySFX(envTransitionClip);
                StartCoroutine(FadeSkybox(Sky[2], Sky[0], 2f));
            }
        }
        else if (Lv == 2 && currentSkyLv != 2)
        {
            currentSkyLv = 2;
            PlaySFX(envTransitionClip);
            StartCoroutine(FadeSkybox(Sky[0], Sky[1], 2f));
        }
        else if (Lv >= 3 && currentSkyLv != 3)
        {
            currentSkyLv = 3;
            PlaySFX(envTransitionClip);
            StartCoroutine(FadeSkybox(Sky[1], Sky[2], 2f));
        }

        //Lv2
        if (Lv == 2)
        {
            if (Lv2Swap)
            {
                SpawnPoint[0].transform.position = LvTwo[0].transform.position;
                SpawnPoint[1].transform.position = LvTwo[3].transform.position;
                SpawnPoint[2].transform.position = LvTwo[4].transform.position;
            }
            else
            {
                SpawnPoint[0].transform.position = LvTwo[0].transform.position;
                SpawnPoint[1].transform.position = LvTwo[1].transform.position;
                SpawnPoint[2].transform.position = LvTwo[2].transform.position;
            }
        }
        //Lv3 & Endless
        else if (Lv >= 3)
        {
            MoveSpawnPointUpDown(SpawnPoint[1], ref moveUpOne, fixedZ, LvTwo[1], LvTwo[3]);
            MoveSpawnPointUpDown(SpawnPoint[2], ref moveUpTwo, fixedZ, LvTwo[4], LvTwo[2]);
        }

        if (Life <= 0)
        {
            YouLose.SetActive(true);
            if(highScore < BuildingCount)
            {
                highScore = BuildingCount;
            }
            Time.timeScale = 0f;
        }
    }

    //วิธีขยับของ Lv3 & Endless
    private void MoveSpawnPointUpDown(GameObject spawn, ref bool moveUp, float fixedLocalZ, GameObject minPoint, GameObject maxPoint)
    {
        Transform sp = spawn.transform;
        Vector3 localPos = sp.localPosition;
        if (localPos.y >= maxPoint.transform.localPosition.y)
            moveUp = false;
        else if (localPos.y <= minPoint.transform.localPosition.y)
            moveUp = true;
        float direction = moveUp ? 1f : -1f;
        localPos.y += direction * 2f * Time.deltaTime;
        localPos.z = fixedLocalZ;
        sp.localPosition = localPos;
    }

    //เปลี่ยน Bg แบบ Fade
    IEnumerator FadeSkybox(Material fromSky, Material toSky, float duration)
    {
        float t = 0f;
        Material skyA = new Material(fromSky);
        Material skyB = new Material(toSky);

        RenderSettings.skybox = skyA;

        DynamicGI.UpdateEnvironment();

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = 1f - (t / duration);
            float b = (t / duration);
            skyA.SetFloat("_Exposure", a);
            skyB.SetFloat("_Exposure", b);
            if (b > 0.01f)
                RenderSettings.skybox = skyB;
            DynamicGI.UpdateEnvironment();

            yield return null;
        }
        RenderSettings.skybox = toSky;
        DynamicGI.UpdateEnvironment();
    }

    //
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null)
            return;

        if (bgmSource.clip == clip)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void PlayBGMForLevel(int level)
    {
        if (level <= 1)
        {
            PlayBGM(bgmLv1Clip);
        }
        else if (level == 2)
        {
            PlayBGM(bgmLv2Clip);
        }
        else
        {
            PlayBGM(bgmLv3Clip);
        }
    }
    //

    public void Reset()
    {
        Lv = 1;
        Life = 3;
        BuildingCount = 0;
    }
}

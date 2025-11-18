
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
    private void Start()
    {
        Life = 3;
        boots = false;
        fixedZ = SpawnPoint[1].transform.localPosition.z;
        RenderSettings.skybox = Sky[0];
        DynamicGI.UpdateEnvironment();
    }
    void Update()
    {
        //นับว่าวางตึกติดกันโดยที่ไม่ตก
        if (PerfectCount == 10)
        {
            boots = true;
        }
        //ดูว่าBootsหมดยัง
        if (bootsTimer >= 5)
        {
            boots = false;
            PerfectCount = 0;
            bootsTimer = 0;
        }
        //สร้างตึกตอนBoots
        if (!JustReset && boots && !manager.Reset)
        {
            bootsTimer += Time.deltaTime;
            if (!HasBuilding)
            {
                Instantiate(Cube[0], SpawnPoint[0].transform.position, Quaternion.identity);
                BuildingHeight = Cube[random].transform.localScale.y;
                HasBuilding = true;
                IsMoving = false;
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
            }
        }
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
            }
        }
        //กดจอวางตึก
        if (Input.touchCount > 0 && HasBuilding)
        {
            IsMoving = true;
        }
        //เปลี่ยนLv
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
        //เปลี่ยนBg
        if (Lv == 1 && currentSkyLv != 1)
        {
            if(currentSkyLv == 2)
            {
                currentSkyLv = 1;
                StartCoroutine(FadeSkybox(Sky[1], Sky[0], 2f));
            }else if (currentSkyLv >= 3)
            {
                currentSkyLv = 1;
                StartCoroutine(FadeSkybox(Sky[2], Sky[0], 2f));
            }          
        }
        else if (Lv == 2 && currentSkyLv != 2)
        {
            currentSkyLv = 2;
            StartCoroutine(FadeSkybox(Sky[0], Sky[1], 2f));
        }
        else if (Lv >= 3 && currentSkyLv != 3)
        {
            currentSkyLv = 3;
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
            Time.timeScale = 0f;
        }
        
    }
    //วิธีขยับของLv3 & Endless
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
    //เปลี่ยนBgแบบFade
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
    public void Reset()
    {
        Lv = 1;
        Life = 3;
        BuildingCount = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;

public class Spawner : MonoBehaviour
{
    public bool isTest = false;
    public static bool isTestStatic = false;
    public static bool isPause = false;
    public static int drainingMosqs = 0;
    public static int killedMosqs = 0;
    public static bool isTargetSkin = true;
    public static bool gameStarted = false;
    public static bool gameCleared = false;
    public RectTransform canvas;
    float widthDiv;
    float heightDiv;
    public Vector2[] slingshotGrid = new Vector2[45];
    public GameObject slingshot;
    public GameObject mosq;
    public int mosqSpd;

    float timer = 0;
    float spawnInterval = 0;
    int spawningAtOnce = 1;
    bool stopSpawn = false;
    int spawningSlingshotIdx = 0;
    int waitingTime = 2;

    public int matchingIdx = 0;
    public Dictionary<int, GameObject> slingshotWithIdx = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> mosqWithIdx = new Dictionary<int, GameObject>();

    public enum Phase { phase_1, phase_2, phase_3, phase_clear }
    public static Phase phase = Phase.phase_1;

    void Start()
    {
        initSlingshot();
        StartCoroutine(slingshotSpawnLoop());
        shuffleArray(ref slingshotGrid);
    }

    void initSlingshot()
    {
        widthDiv = canvas.sizeDelta.x / 9;
        heightDiv = canvas.sizeDelta.y / 5;
        int slingshotIdx = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                slingshotGrid[slingshotIdx] = new Vector2(
                    widthDiv * i + widthDiv / 2,
                    heightDiv * j + heightDiv / 2
                );
                slingshotIdx++;
            }
        }
    }

    private void Update()
    {
        if(isTest)
        {
            isTestStatic = isTest;
        }
       
        if(gameStarted && !gameCleared)
        {
            switch(phase)
            {
                case Phase.phase_1 :
                    spawningAtOnce = 1;
                    if(checkFail()) phase = Phase.phase_1;
                    else checkSuccess();
                    break;
                case Phase.phase_2:
                    spawningAtOnce = 2;
                    if (checkFail()) phase = Phase.phase_1;
                    else checkSuccess();
                    break;
                case Phase.phase_3:
                    spawningAtOnce = 3;
                    if (checkFail()) phase = Phase.phase_2;
                    else checkSuccess();
                    break;
                case Phase.phase_clear:
                    spawningAtOnce = 0;
                    StopCoroutine(slingshotSpawnLoop());
                    gameCleared = true;
                    break;
            }
        }
    }

    bool checkFail()
    {
        if(drainingMosqs > 2)
        {
            StartCoroutine(pauseSpawn());
            killedMosqs = 0;
            drainingMosqs = 0;
            return true;
        }
        return false;
    }

    void checkSuccess()
    {
        switch (phase)
        {
            case Phase.phase_1:
                if(killedMosqs > 1)
                {
                    phase = Phase.phase_2;
                    StartCoroutine(pauseSpawn());
                }
                break;
            case Phase.phase_2:
                if (killedMosqs > 5)
                {
                    phase = Phase.phase_3;
                    StartCoroutine(pauseSpawn());
                }
                break;
            case Phase.phase_3:
                if (killedMosqs > 9)
                {
                    phase = Phase.phase_clear;
                    StartCoroutine(pauseSpawn());
                }
                break;
        }
    }

    IEnumerator pauseSpawn()
    {
        waitingTime = 3;
        killedMosqs = 0;
        drainingMosqs = 0;
        isPause = true;
        yield return new WaitForSeconds(2f);
        foreach(var entry in mosqWithIdx)
        {
            if(entry.Value != null)
            {
                Destroy(entry.Value);
            }
        }

        foreach (var entry in slingshotWithIdx)
        {
            if (entry.Value != null)
            {
                Destroy(entry.Value);
            }
        }
        isPause = false;
    }


    IEnumerator slingshotSpawnLoop()
    {
        while(gameStarted && !gameCleared)
        {
            yield return new WaitForSeconds(waitingTime);
            if(waitingTime != 2)
            {
                waitingTime = 2;
                isTargetSkin = false;
            }
            else
            {
                isTargetSkin = true;
                spawnMosqAndSlingshot(spawningAtOnce);
            }
        }
    }

    void spawnMosqAndSlingshot(int num)
    {
        if(spawningSlingshotIdx >= slingshotGrid.Length - 1)
        {
            shuffleArray(ref slingshotGrid);
            spawningSlingshotIdx = 0;
        }

        for(int j = spawningSlingshotIdx; j < spawningSlingshotIdx + num; j++)
        {
            mosqWithIdx.Add(matchingIdx, Instantiate(mosq));
            slingshotWithIdx.Add(matchingIdx, Instantiate(slingshot));

            GameObject s = slingshotWithIdx[matchingIdx];
            s.transform.SetParent(canvas);
            s.GetComponent<Slingshot>().slingshotId = matchingIdx;
            s.AddComponent<RectTransform>();
            s.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            s.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            s.GetComponent<RectTransform>().anchoredPosition = slingshotGrid[j];

            GameObject m = mosqWithIdx[matchingIdx];
            m.GetComponent<Mosq>().stdSpd = mosqSpd;
            m.transform.SetParent(canvas);
            m.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            m.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            Vector2 spawnPoint = Vector2.zero;
            while((spawnPoint.x > -150 && spawnPoint.x < 2150) && (spawnPoint.y > -150 && spawnPoint.y < 1220))
            {
                spawnPoint = new Vector2(
                    Random.Range(-400, 2450),
                    Random.Range(-450, 1620)
                );
            }
            m.GetComponent<RectTransform>().anchoredPosition = spawnPoint;

            matchingIdx++;
        }

        spawningSlingshotIdx += num;
    }

    void shuffleArray(ref Vector2[] arr)
    {
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
    }
}

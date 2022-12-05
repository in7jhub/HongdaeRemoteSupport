using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;

public class Spawner : MonoBehaviour
{
    public static bool gameStarted = false;
    public static bool gameCleared = false;
    public RectTransform canvas;
    float widthDiv;
    float heightDiv;
    public Vector2[] slingshotGrid = new Vector2[45];
    public Dictionary<int, bool> slingshotUsedInfo = new Dictionary<int, bool>();
    public GameObject slingshot;
    public GameObject mosq;

    float timer = 0;
    float spawnInterval = 0;
    int spawningAtOnce = 1;
    bool stopSpawn = false;
    int spawningSlingshotIdx = 0;

    public static int drainingMosqs = 0;
    public static bool isTargetSkin = true;

    public enum Phase { phase_1, phase_2, phase_3, phase_clear }
    public static Phase phase = Phase.phase_1;

    void Start()
    {
        gameStarted = true;
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

                slingshotUsedInfo.Add(slingshotIdx, false);
                slingshotIdx++;
            }
        }
    }

    void Update()
    {

    }

    IEnumerator slingshotSpawnLoop()
    {
        while(gameStarted && !gameCleared)
        {
            yield return new WaitForSeconds(1f);
            spawn(2);
        }
    }

    void spawn(int num)
    {
        GameObject[] obj = new GameObject[num];
        
        if(spawningSlingshotIdx >= slingshotGrid.Length - 1) return;
        for(int j = 0; j < num; j++)
        {
            for(int i = spawningSlingshotIdx; i < spawningSlingshotIdx + num; i++)
            {
                obj[j] = Instantiate(slingshot);
                obj[j].AddComponent<RectTransform>();
                obj[j].transform.SetParent(canvas);
                obj[j].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                obj[j].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                obj[j].GetComponent<RectTransform>().anchoredPosition = slingshotGrid[i];
            }
        }

        spawningSlingshotIdx += num;
    }

    void shuffleArray(ref Vector2[] arr)
    {
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
    }
}

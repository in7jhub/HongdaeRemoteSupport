using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;

public class SetOfMosqSet : MonoBehaviour
{
    public static bool gameStarted = false;
    public static bool gameCleared = false;
    public static bool test = false;

    float timer = 0;
    float spawnInterval = 0;
    int spawningAtOnce = 1;
    float timeRecord = 0;
    int spawningMosqIdx = 0;
    bool hasIntervalChance = true;
    int[] randomQueue;
    public bool stopSpawn = false;

    public static int killedMosqs = 0;
    public static int spawnedMosqs = 0;
    public static int drainingMosqs = 0;
    public static bool isTargetSkin = true;

    public GameObject[] mosqSets;
    public enum Phase {phase_1, phase_2, phase_3, phase_clear}
    public static Phase phase = Phase.phase_1;
    
    void Start()
    {
        hasIntervalChance = true;
        spawnedMosqs = 0;
        mosqSets = new GameObject[transform.childCount];
        randomQueue = new int[transform.childCount];
    
        for(int i = 0; i < transform.childCount; i++)
        {
            randomQueue[i] = i;
            mosqSets[i] = transform.GetChild(i).gameObject;
            mosqSets[i].SetActive(false);
        }

        gameStarted = false;
    }

    void Update()
    {
        if(gameStarted && !gameCleared)
        {
            timer += Time.deltaTime;
            switch(phase)
            {
                case Phase.phase_1 :
                    spawningAtOnce = 1;
                    phaseControl();
                    break;

                case Phase.phase_2:
                    spawningAtOnce = 2;
                    phaseControl();
                    break;

                case Phase.phase_3:
                    spawningAtOnce = 3;
                    phaseControl();
                    break;

                case Phase.phase_clear:
                    break;
            }
        }
    }

    void phaseControl()
    {
        if (!stopSpawn)
        {
            setSpawnIntervalAndQueue(2f);
            spawn();
            phaseDown(checkPhaseFail());
            phaseUp(checkPhaseClear());
        }
    }

    void setSpawnIntervalAndQueue(float intervalSec)
    {
        // 1 deltim = 1/20 sec
        //          = 50 millis
        // 20 deltim = 1 sec 
        //           = 1000 millis
        if(hasIntervalChance)
        {
            spawnedMosqs = 0;
            spawnInterval = intervalSec;
            timeRecord = timer;
            shuffleArray(ref randomQueue);
            hasIntervalChance = false;
        }
    }

    void spawn()
    {
        if(isTargetSkin && timer > spawnInterval && !stopSpawn)
        {
            timer = 0;

            if (spawningMosqIdx >= mosqSets.Length - 1) {
                spawningMosqIdx = 0;
                stopSpawn = true;
                StartCoroutine(targetSwap(Phase.phase_1));
                return; 
            }

            for(int i = 0; i < spawningAtOnce; i++)
            {
                mosqSets[randomQueue[spawningMosqIdx]].SetActive(true);
                //slingshot 켜기
                mosqSets[i].transform.GetChild(1).gameObject.SetActive(true);
                spawningMosqIdx += 1;
                spawnedMosqs++;
            }
        }
    }

    void phaseUp(bool go)
    {
        switch (phase)
        {
            case Phase.phase_1:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_2)); 
                }
                break;
            case Phase.phase_2:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_3)); 
                }
                break;
            case Phase.phase_3:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_clear)); 
                } 
                break;
            case Phase.phase_clear:
                gameCleared = true;
                break;
        }
    }

    void phaseDown(bool go)
    {
        switch (phase)
        {
            case Phase.phase_1:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_1)); 
                }
                break;
            case Phase.phase_2:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_1)); 
                }
                break;
            case Phase.phase_3:
                if (go) {
                    StartCoroutine(targetSwap(Phase.phase_2)); 
                }
                break;
            case Phase.phase_clear:
                gameCleared = true;
                break;
        }
    }

    bool checkPhaseFail()
    {
        switch (phase)
        {
            case Phase.phase_1:
                return drainingMosqs >= 2;
            case Phase.phase_2:
                return drainingMosqs >= 2;
            case Phase.phase_3:
                return drainingMosqs >= 2;
            default :
                return false;
        }
    }

    bool checkPhaseClear()
    {
        switch (phase)
        {
            case Phase.phase_1:
                return killedMosqs >= 2;
            case Phase.phase_2:
                return killedMosqs >= 6;
            case Phase.phase_3:
                return killedMosqs >= 12;
            default :
                return false;
        }
    }

    void shuffleArray(ref int[] arr)
    {
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
    }

    void changePhase(Phase _phase)
    {
        phase = _phase;
        spawningMosqIdx = 0;
        drainingMosqs = 0;
        killedMosqs = 0;
        spawnedMosqs = 0;
        for(int i = 0; i < mosqSets.Length; i++)
        {
            MosqSet mosqset = mosqSets[i].GetComponent<MosqSet>();
            //mosqset 전부끄기
            mosqset.gameObject.SetActive(false);
            //모기 제자리로
            mosqset.targetMosq.transform.position = mosqset.targetMosq.GetComponent<Mosq>().startingPoint;
            //모기 피 drain 기회를 주는 flag
            mosqset.targetMosq.GetComponent<Mosq>().isDrainingFlag = true;

            //피 drain 효과 끄기
            mosqset.targetMosq.GetComponent<Mosq>().bloodEffect.SetActive(false);

            //slingshot 켜기
            GameObject slingshot = mosqset.slingshot.gameObject;
            slingshot.SetActive(true);
            //bang끄기
            slingshot.GetComponent<Slingshot>().slingshotBang.SetActive(false);
            //slingshot 타이머 초기화
            mosqset.wakeSlingshot();
            //shotFlag 켜기 (총알장전)
            slingshot.GetComponent<Slingshot>().shotFlag = true;
            slingshot.GetComponent<Slingshot>().size = slingshot.GetComponent<Slingshot>().initSize;
            //모기 켜기
            mosqset.targetMosq.gameObject.SetActive(true);
        }
        hasIntervalChance = true;
    }

    IEnumerator targetSwap(Phase _phase)
    {
        stopSpawn = true;
        drainingMosqs = 0;
        killedMosqs = 0;
        spawnedMosqs = 0;
        spawningMosqIdx = 0;
        yield return new WaitForSeconds(1f);
        isTargetSkin = false;
        yield return new WaitForSeconds(1f);
        changePhase(_phase);
        stopSpawn = false;
        isTargetSkin = true;
    }
}

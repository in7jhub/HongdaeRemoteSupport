using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] exitButtons;

    public GameObject forceClear;
    int gameClearRqst = 0;

    public GameObject forceGameStart;
    int gameStartRqst = 0;

    public GameObject blueScreen;
    int wholeGameRqst = 0;

    private void Awake()
    {
        Application.targetFrameRate = 30;
        blueScreen.SetActive(false);
        forceClear.SetActive(false);
        forceGameStart.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F11))
        {
            gameClearRqst++;
        }

        if(gameClearRqst > 10 && Input.GetKeyDown(KeyCode.F1))
        {
            forceClear.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            gameStartRqst++;
        }

        if (gameStartRqst > 10 && Input.GetKeyDown(KeyCode.F1))
        {
            forceGameStart.SetActive(true);
            for(int i = 0; i < exitButtons.Length; i++)
            {
                exitButtons[i].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            wholeGameRqst++;
        }

        if (wholeGameRqst > 10 && Input.GetKeyDown(KeyCode.F1))
        {
            blueScreen.SetActive(true);
            LastSceneText.wholeGameCompleted = true;
        }
    }
}

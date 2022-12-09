using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableToForceStart : MonoBehaviour
{
    public Text lastSceneText;
    public GameObject lastScene;
    public Image disconnectImg;
    public GameObject video;
    public GameObject[] exitButtons;

    void OnEnable()
    {
        video.SetActive(false);
        disconnectImg.enabled = true;
        lastSceneText.enabled = true;
        lastScene.GetComponent<Image>().enabled = true;
        Spawner.gameStarted = true;
        exitButtons = transform.parent.GetComponent<GameManager>().exitButtons;
        for(int i = 0; i < exitButtons.Length; i++)
        {
            exitButtons[i].SetActive(true);
        }
    }
}

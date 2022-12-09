using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Disconnection : MonoBehaviour
{
    public double time;
    public double currentTime;
    public bool initFlag = true;
    public VideoPlayer vidp;
    public GameObject exit_1;
    public GameObject exit_2;
    public GameObject lastScene;
    public Text lastSceneText;
    public GameObject spawner;
    // Use this for initialization
    void Start()
    {
        lastSceneText.enabled = false;
        lastScene.GetComponent<Image>().enabled = false;
        vidp.loopPointReached += CheckOver;
        time = vidp.GetComponent<VideoPlayer>().clip.length;
        gameObject.GetComponent<Image>().enabled = false;
        exit_1.SetActive(false);
        exit_2.SetActive(false);
        // lastScene.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (vidp.frame == (long)vidp.frameCount)
        {
            if(initFlag)
            {
                lastSceneText.enabled = true;
                lastScene.GetComponent<Image>().enabled = true;
                gameObject.GetComponent<Image>().enabled = true;
                exit_1.SetActive(true);
                exit_2.SetActive(true);
                initFlag = false;
            }
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        if (initFlag)
        {
            gameObject.GetComponent<Image>().enabled = true;
            exit_1.SetActive(true);
            exit_2.SetActive(true);
            initFlag = false;
        }
    }

    public void disable()
    {
        gameObject.GetComponent<Image>().color = new Color(
            gameObject.GetComponent<Image>().color.r,
            gameObject.GetComponent<Image>().color.g,
            gameObject.GetComponent<Image>().color.b,
            0.5f
        );
        Invoke("disableDelay", 0.5f);
    }

    public void disableDelay()
    {
        lastSceneText.enabled = true;
        lastScene.GetComponent<Image>().enabled = true;
        Spawner.gameStarted = true;
        spawner.SetActive(true);
        gameObject.SetActive(false);
    }
}

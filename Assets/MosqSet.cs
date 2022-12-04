using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MosqSet : MonoBehaviour
{
    public Transform slingshot;
    public Transform targetMosq;
    public GameObject bullet;

    public void shotMosq()
    {
        GameObject bulletInstance = Instantiate(bullet);

        bulletInstance.transform.position = slingshot.transform.position;

        bulletInstance.AddComponent<Rigidbody>();
        bulletInstance.GetComponent<Bullet>().setTargetMosq(targetMosq);
        bulletInstance.GetComponent<Rigidbody>().useGravity = false;

        slingshotBangWake();
        sleepSlingshot();
    }

    public void sleepSlingshot()
    {
        Invoke("delaySlingshotBangSleep", 1f);
        slingshot.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        slingshot.GetComponent<Slingshot>().timerBoundImg.gameObject.SetActive(false);
        slingshot.GetComponent<Slingshot>().timerImg.gameObject.SetActive(false);
    }
    

    public void wakeSlingshot()
    {
        slingshot.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        slingshot.GetComponent<Slingshot>().timerBoundImg.gameObject.SetActive(true);
        slingshot.GetComponent<Slingshot>().timerImg.gameObject.SetActive(true);
    }

    public void delaySlingshotBangSleep()
    {
        slingshot.GetComponent<Slingshot>().slingshotBang.SetActive(false);
    }

    public void slingshotBangWake()
    {
        slingshot.GetComponent<Slingshot>().slingshotBang.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mosq : MonoBehaviour
{
    RectTransform target;
    public Vector3 startingPoint;
    public bool isDrainingFlag = true;
    public GameObject bloodEffect;
    Rigidbody rb;
    public float stdSpd;
    RectTransform rt;
    public float decSpd = 1;
    float initMag;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        target = GameObject.FindWithTag("Target").GetComponent<RectTransform>();
        startingPoint = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        initMag = (transform.position - target.transform.position).magnitude;
    }

    void FixedUpdate()
    {
        stdSpd = ((transform.position - target.transform.position).magnitude / initMag) * stdSpd + 2;

        if(target.anchoredPosition.x >= rt.anchoredPosition.x)
        {
            rt.localScale = new Vector3(-21.6f, 21.6f, 1f);
            bloodEffect.transform.localPosition = new Vector3(-1.26f, -1.81f, 0);
        }

        if(Spawner.isTargetSkin)
        {
            if ((transform.position - target.transform.position).magnitude > 1f)
            {
                Vector3 norm = (target.transform.position - transform.position).normalized;
                norm = new Vector3(
                    norm.x * stdSpd * Time.deltaTime,
                    norm.y * stdSpd * Time.deltaTime,
                    0
                );
                transform.position += norm;
            }
            else
            {
                bloodEffect.SetActive(true);
                if(isDrainingFlag)
                {
                    Spawner.drainingMosqs++;
                    isDrainingFlag = false;
                }
            }
        }
        else
        {
            stdSpd = 20;
            Vector3 norm = (startingPoint - transform.position).normalized;
            norm = new Vector3(
                norm.x * stdSpd * 2.3f * Time.deltaTime,
                norm.y * stdSpd * 2.3f * Time.deltaTime,
                0
            );

            transform.position += norm;
            bloodEffect.SetActive(false);

            if((startingPoint - transform.position).magnitude < 0.2f)
            {
                isDrainingFlag = true;
                transform.position = startingPoint;
                Destroy(gameObject);
            }
        }
    }
}

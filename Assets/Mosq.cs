using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mosq : MonoBehaviour
{
    GameObject target;
    public Vector3 startingPoint;
    public bool isDrainingFlag = true;
    public GameObject bloodEffect;
    Rigidbody rb;
    public float stdSpd;

    void Start()
    {
        target = GameObject.FindWithTag("Target");
        startingPoint = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(SetOfMosqSet.isTargetSkin)
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
                    SetOfMosqSet.drainingMosqs++;
                    isDrainingFlag = false;
                }
            }
        }
        else
        {
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
                transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}

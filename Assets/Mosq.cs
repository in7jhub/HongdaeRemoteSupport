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

    void Update()
    {
        if(SetOfMosqSet.isTargetSkin)
        {
            if ((transform.position - target.transform.position).magnitude > 1f)
            {
                rb.velocity = (target.transform.position - transform.position).normalized * stdSpd * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector3.zero;
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
            rb.velocity = (transform.position - startingPoint).normalized * stdSpd * 3 * Time.deltaTime;
            bloodEffect.SetActive(false);

            if((transform.position - startingPoint).magnitude < 0.2f)
            {
                rb.velocity = Vector3.zero;
                isDrainingFlag = true;
                transform.position = startingPoint;
                transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}

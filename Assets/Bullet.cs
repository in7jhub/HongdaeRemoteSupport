using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform targetMosq;
    public GameObject hitEffect;
    public float bulletSpd;


    void Update()
    {
        delayedFollow();
        checkHit();
    }

    void delayedFollow()
    {
        transform.GetComponent<Rigidbody>().velocity = (
            targetMosq.transform.position - transform.position
        ).normalized * bulletSpd * Time.deltaTime;
    }

    public void setTargetMosq(Transform _t)
    {
        targetMosq = _t;
    }

    void checkHit()
    {
        Vector2 v1 = transform.position;
        Vector2 v2 = targetMosq.transform.position;
        
        if((v1 - v2).magnitude < 0.45f)
        {
            SetOfMosqSet.killedMosqs++;
            GameObject h = Instantiate(hitEffect);
            h.transform.position = new Vector3(targetMosq.transform.position.x, targetMosq.transform.position.y, 101);
            targetMosq.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}

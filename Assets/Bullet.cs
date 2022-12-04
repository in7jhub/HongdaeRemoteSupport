using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform targetMosq;
    public GameObject hitEffect;
    public float bulletSpd;

    void FixedUpdate()
    {
        Vector2 v1 = transform.position;
        Vector2 v2 = targetMosq.transform.position;

        if ((v1 - v2).magnitude < 0.45f)
        {
            SetOfMosqSet.killedMosqs++;
            GameObject h = Instantiate(hitEffect);
            h.transform.position = new Vector3(targetMosq.transform.position.x, targetMosq.transform.position.y, 101);
            targetMosq.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Vector3 norm = (v2 - v1).normalized;
            norm = new Vector3(
                norm.x * bulletSpd * Time.deltaTime,
                norm.y * bulletSpd * Time.deltaTime,
                0
            );
            transform.position += norm;
        }
    }

    public void setTargetMosq(Transform _t)
    {
        targetMosq = _t;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slingshot : MonoBehaviour
{
    public SpriteRenderer timerImg;
    public SpriteRenderer timerBoundImg;
    public GameObject slingshotBang;
    public Vector2 size;
    public Vector2 initSize;
    public float shrinkSpd = 0;
    public GameObject bullet;
    public int slingshotId;

    private void Start()
    {
        size = timerImg.transform.localScale;
        initSize = size;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !Spawner.isPause)
        {
            GameObject target = GetClickedObject();
            if(target != null && target.Equals(gameObject))
            {
                shotMosq();
                Destroy(gameObject);
            }
        }

        if (Spawner.phase == Spawner.Phase.phase_1)
        {
            shrinkSpd = 0.9f * 1.5f;
            if(Spawner.isTestStatic)
            {
                shrinkSpd = 0.2f;
            }
        }
        else if (Spawner.phase == Spawner.Phase.phase_2)
        {
            shrinkSpd = 0.9f * 1.6f;
            if (Spawner.isTestStatic)
            {
                shrinkSpd = 0.2f;
            }
        }
        else if (Spawner.phase == Spawner.Phase.phase_3)
        {
            shrinkSpd = 0.9f * 1.72f;
            if (Spawner.isTestStatic)
            {
                shrinkSpd = 0.2f;
            }
        }

        size = new Vector2(size.x - shrinkSpd * Time.deltaTime, size.y - shrinkSpd * Time.deltaTime);
        timerImg.transform.localScale = size;

        if(size.x < initSize.x * 0.25f)
        {
            timerImg.transform.localScale = initSize;
            Destroy(gameObject);
        }
    }

    public void shotMosq()
    {
        GameObject bulletInstance = Instantiate(bullet);
        Spawner spawner = GameObject.Find("Spawner").GetComponent<Spawner>();

        bulletInstance.transform.position = transform.position;
        bulletInstance.AddComponent<Rigidbody>();
        bulletInstance.GetComponent<Bullet>().setTargetMosq(
            spawner.mosqWithIdx[slingshotId].transform
        );

        bulletInstance.GetComponent<Rigidbody>().useGravity = false;
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        //마우스 포인트 근처 좌표를 만든다. 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        //마우스 근처에 오브젝트가 있는지 확인
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   
        {
            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
        }
        return target;
    }

    private void OnEnable()
    {
        wakeSlingshot();
    }

    public void sleepSlingshot()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        timerBoundImg.gameObject.SetActive(false);
        timerImg.gameObject.SetActive(false);
    }

    public void wakeSlingshot()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        timerBoundImg.gameObject.SetActive(true);
        timerImg.gameObject.SetActive(true);
    }
}

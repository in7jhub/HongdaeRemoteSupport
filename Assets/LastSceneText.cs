using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastSceneText : MonoBehaviour
{
    bool flag = true;
    bool narrationFlag = false;
    string rmbtext = "";
    InputField field;
    bool quiz = false;
    bool flag2 = true;
    public float flipSpd;
    public RectTransform fan;
    Rigidbody fanRb;
    public RectTransform fanDestin;
    Vector3 fanStartingPos;
    bool flipped = false;
    bool rightAnswer = false;
    public static bool wholeGameCompleted = false;
    public Text skinText;
    public GameObject setOfmosqSet;

    public static bool isFlipped = false;

    float blueScreenTimer = 0;
    public GameObject bluescreen;

    public float fanSpd;

    void Start()
    {
        fanRb = fan.transform.GetComponent<Rigidbody>();
        field = gameObject.transform.parent.gameObject.GetComponent<InputField>();
        field.onValidateInput += delegate(string text, int charIndex, char addedChar)
        {
            return changeUpperCase(addedChar);
        };
    }


    private void Update()
    {
        if(flipped && rightAnswer)
        {
            blueScreenTimer+=Time.deltaTime;
            if(blueScreenTimer > 4)
            {
                wholeGameCompleted = true;
                bluescreen.SetActive(true);
            }
        }

        if(field.text.Length <= 4)
        {
            rmbtext = field.text;
        }

        if(SetOfMosqSet.gameCleared && flag)
        {
            setOfmosqSet.SetActive(false);

            flag = false;
            field.text = "";
            field.interactable = true;
            field.gameObject.GetComponent<RectTransform>().localScale = new Vector3(
                -1 * field.gameObject.GetComponent<RectTransform>().localScale.x,
                field.gameObject.GetComponent<RectTransform>().localScale.y,
                field.gameObject.GetComponent<RectTransform>().localScale.z
            );

            //코드가 없어졌어요 나레이션
            

            //flag는 아 이건 뭐야! 아 이걸 뒤집어야하는데;;
            quiz = true;
            narrationFlag = true;
        }

        if(narrationFlag && field.text.Length > 1)
        {
            // 아 이건 뭐야! 아 이걸 뒤집어야하는데;;
            isFlipped = true; // 이 불린값은 오디오 제어에 쓰임
            narrationFlag = false;
        }

        if (SetOfMosqSet.gameCleared && field.text == "SKIN")
        {
            rightAnswer = true;
            skinText.color = Color.green;
            field.interactable = false;
            field.text = "SKIN";
        }

        // USB 매크로로 F12 -> F1 인풋 받으면 선풍기 나옴
        if(Input.GetKeyDown(KeyCode.F7) && flag2)
        {
            quiz = true;
            flag2 = false;
        }

        // 선풍기 연출 시작 
        if(quiz && Input.GetKeyDown(KeyCode.F4) && SetOfMosqSet.gameCleared)
        {
            quiz = false;
            StartCoroutine(showFan());
        }
    }

    IEnumerator showFan()
    {
        fan.anchoredPosition = new Vector2(1640, fan.anchoredPosition.y);
        fanStartingPos = fan.GetComponent<RectTransform>().anchoredPosition;
        fan.gameObject.SetActive(true);
        bool flipstart = true;

        fanRb.velocity =  (fanDestin.anchoredPosition - fan.GetComponent<RectTransform>().anchoredPosition).normalized * fanSpd * Time.deltaTime;

        while(fan.anchoredPosition.x > 149)
        {
            if(flipstart && fan.anchoredPosition.x < 500)
            {
                StartCoroutine(flipText());
                flipstart = false;
            }

            yield return null;
        }

        fanRb.velocity = Vector3.zero;

        yield return new WaitForSeconds(3.5f);

        fanRb.velocity = ((Vector2)fanStartingPos - (Vector2)fan.GetComponent<RectTransform>().anchoredPosition).normalized * fanSpd * Time.deltaTime;
        yield return null;
    }

    IEnumerator flipText()
    {
        if(field.text == "")
        {
            field.text = "FLIP";
        }

        while(field.transform.localScale.x < 1)
        {
            field.transform.localScale = new Vector3(
                field.transform.localScale.x + 0.01f * flipSpd * Time.deltaTime, field.transform.localScale.y, field.transform.localScale.z
            ); 
            yield return null;
        }

        while (field.transform.localScale.x > -1)
        {
            field.transform.localScale = new Vector3(
                field.transform.localScale.x + -0.01f * flipSpd * Time.deltaTime, field.transform.localScale.y, field.transform.localScale.z
            );
            yield return null;
        }

        while (field.transform.localScale.x < 1)
        {
            field.transform.localScale = new Vector3(
                field.transform.localScale.x + 0.01f * flipSpd * Time.deltaTime, field.transform.localScale.y, field.transform.localScale.z
            );
            yield return null;
        }

        while (field.transform.localScale.x > -1)
        {
            field.transform.localScale = new Vector3(
                field.transform.localScale.x + -0.01f * flipSpd * Time.deltaTime, field.transform.localScale.y, field.transform.localScale.z
            );
            yield return null;
        }

        while (field.transform.localScale.x < 1)
        {
            field.transform.localScale = new Vector3(
                field.transform.localScale.x + 0.01f * flipSpd * Time.deltaTime, field.transform.localScale.y, field.transform.localScale.z
            );
            yield return null;
        }

        flipped = true;
    }

    char changeUpperCase(char _cha)
    {
        char tmpChar = _cha;
        string tmpString = tmpChar.ToString();
        tmpString = tmpString.ToUpper();
        tmpChar = System.Convert.ToChar(tmpString);
        return tmpChar;
    }
}

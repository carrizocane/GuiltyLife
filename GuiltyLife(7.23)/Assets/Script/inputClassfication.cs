using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputClassfication : MonoBehaviour
{
    bool claOn = false;
    public GameObject claObj;
    public static int cla = 0;
        
    public void inputClassificationOnOff()
    {
        GameObject claOnOffObj = claObj.transform.Find("inputClassification (list)").gameObject;

        if (claOn == true)
            claOnOffObj.SetActive(false); //꺼짐
        else
            claOnOffObj.SetActive(true); //켜짐
        claOn = !claOn;
    }

    public void touchClassfication()
    {
        UILabel inputCla = claObj.transform.Find("inputClassification (title)").gameObject.transform.Find("Label").GetComponent<UILabel>();
        Debug.Log("터치해서 분류 바꿔줌");
        if (this.name[0] != 'i') {
            switch (this.name[15]) {
                case '0':
                    cla = 0;
                    inputCla.text = "기타";
                    break;
                case '1':
                    inputCla.text = "과제 및 업무";
                    cla = 1;
                    break;
                case '2':
                    inputCla.text = "약속";
                    cla = 2;
                    break;
                case '3':
                    inputCla.text = "일상 속 할일";
                    cla = 3;
                    break;
                case '4':
                    inputCla.text = "작은 습관";
                    cla = 4;
                    break;
                default:
                    Debug.Log("이럴리없는뎅..");
                    break;
            }
            GameObject claOnOffObj = claObj.transform.Find("inputClassification (list)").gameObject;
            claOnOffObj.SetActive(false); //꺼짐
        }
    }
}
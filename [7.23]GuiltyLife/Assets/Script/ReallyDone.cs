using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallyDone : MonoBehaviour {
    
    public static bool workFlag;
    public static bool listItemFlag;

    public static int index;
    public static string type;

    void Awake() {
        workFlag = false;
        listItemFlag = false;
        type = null;
        index = 0;
    }

    public void reallyDone() {
        if (workFlag) //work에서 온 경우
        {
            Debug.Log("ReallyDone.cs의 reallyDone()에서 work인 경우");
            workFlag = true; //정말 했다니 true로 만들고

            //돌려보냄
            GM.reallyDone.SetActive(false);
            GM.calendarList.SetActive(true);

        }
        else if (listItemFlag) //listItem에서 온 경우
        {
            Debug.Log("ReallyDone.cs의 reallyDone()에서 listItem인 경우");
            listItemFlag = true;//true로 만들고

            //돌려보냄
            GM.reallyDone.SetActive(false);
            GM.myRoom.SetActive(true);
            GM.leftSlide.SetActive(true);
        }
    }

    public void cancel()
    {
        Debug.Log("취소 눌리긴 함");
        if (workFlag) //work에서 눌러서 활성화 된 경우
        {
            workFlag = false; // flag를 false로 만들고

            GM.reallyDone.SetActive(false);
            GM.calendarList.SetActive(true);
        }
        else if (listItemFlag) //listItem에서 눌러서 활성화 된 경우
        {
            listItemFlag = false; //flag를 false로 만들고

            //돌려보냄
            GM.reallyDone.SetActive(false);
            GM.myRoom.SetActive(true);
            GM.leftSlide.SetActive(true);

        }
    }

}

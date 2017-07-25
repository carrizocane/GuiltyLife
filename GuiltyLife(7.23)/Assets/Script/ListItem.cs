using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItem : MonoBehaviour
{

    // 수행하지 않은 일정 터치 -> 일정을 한 것으로 바꿈 (체크 아이콘 on)
    // 이미 수행한 일정 터치 -> 아무 일 일어나지 않음

    void OnClick()
    {
        if (!this.transform.Find("checkIcon").gameObject.activeSelf)
        {
            itemOnClick(); //함수 불러주고
            ReallyDone.listItemFlag = true;
            //reallydone활성화
            GM.leftSlide.SetActive(false);
            GM.myRoom.SetActive(false);
            GM.reallyDone.SetActive(true);
        }
    }

    public void itemOnClick()
    {
        Debug.Log("[ item on click called ]");

        List<Schedule> basicScheduleList = GM.loadSaveManager.getBasicSchedules(); //기본 스케쥴
        List<Schedule> userScheduleList = GM.loadSaveManager.getSchedulesWithDate(Calendar.currentDateString); //유저 스케쥴

        UILabel label = (UILabel)this.transform.Find("Label").GetComponent("UILabel");

        if ("BasicGrid" == this.transform.parent.name) //기본퀘일때
        {
            Debug.Log("기본퀘이다");
            ReallyDone.type = "BASIC";
            for (ReallyDone.index=0; ReallyDone.index < basicScheduleList.Count; ReallyDone.index++)
            {
                if (label.text.Equals(basicScheduleList[ReallyDone.index].getContent())) //일정이랑 이름 같으면 체크하려고..
                {                                                                         //이때 문제는 같은일정 있으면 x함
                     Debug.Log(ReallyDone.index);
                    break;
                }
            }
            if (ReallyDone.index == basicScheduleList.Count) //만약 이상황이면 끝까지 돈거라서 없는거임, 이게 실행될리 없다
            {
                ReallyDone.index = -1;
                return;
            }
        }
        else if ("UserGrid" == this.transform.parent.name) //유저퀘일때
        {
            Debug.Log("유저퀘이다");
            ReallyDone.type = "USER";
            for (ReallyDone.index=0 ; ReallyDone.index < userScheduleList.Count; ReallyDone.index++)
            {
                if (label.text.Equals(userScheduleList[ReallyDone.index].getContent())) //내용 같으면
                {
                    Debug.Log(ReallyDone.index);
                    break;
                }
            }
            if (ReallyDone.index == userScheduleList.Count) //실행될리없다
            {
                ReallyDone.index = -1;
                return;
            }
        }
    }

}

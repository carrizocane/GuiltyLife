using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Work : MonoBehaviour
{
    public static int deleteNum = 0; //static으로 deleteWork로 뭐 삭제할지 넘겨주려고 만든 변수임

    public void deleteButton() {
        Debug.Log("Work.cs의 deleteButton()");
        deleteNum = this.name[6] - 48 - 1; //-1인이유는 배열이라서
        GM.calendarList.SetActive(false);
        GM.deleteWork.SetActive(true);
    }

    void OnClick()
    {
        Debug.Log("Work를 터치함(클릭함)");
        if (!this.transform.Find("check").gameObject.activeSelf) //체크 안될때만 실행할거
        {
            if (Calendar.currentDate == Calendar.tempDate) //현재 날짜만 체크 가능
            {
                itemOnClick();
                ReallyDone.workFlag = true;
                GM.calendarList.SetActive(false);
                GM.reallyDone.SetActive(true);
            }
        }
    }

    public void itemOnClick()
    {
        Debug.Log("[ item on click called ]");

        string tempDateString; 
        if (Calendar.tempDate.Month >= 10 && Calendar.tempDate.Day >= 10)
        {
            tempDateString = Calendar.tempDate.Year + "-" + Calendar.tempDate.Month + "-" + Calendar.tempDate.Day;
        }
        else if (Calendar.tempDate.Month >= 10)
        {
            tempDateString = Calendar.tempDate.Year + "-" + Calendar.tempDate.Month + "-0" + Calendar.tempDate.Day;
        }
        else if (Calendar.tempDate.Day >= 10)
        {
            tempDateString = Calendar.tempDate.Year + "-0" + Calendar.tempDate.Month + "-" + Calendar.tempDate.Day;
        }
        else
        {
            tempDateString = Calendar.tempDate.Year + "-0" + Calendar.tempDate.Month + "-0" + Calendar.tempDate.Day;
        }

        List<Schedule> userScheduleList = GM.loadSaveManager.getSchedulesWithDate(tempDateString); //유저 스케쥴

        UILabel label = (UILabel)this.transform.Find("Label").GetComponent("UILabel");

        for (; ReallyDone.index < userScheduleList.Count; ReallyDone.index++)
        {
            if (label.text.Equals(userScheduleList[ReallyDone.index].getContent())) //내용 같으면
            {
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

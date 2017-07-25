using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddWork : MonoBehaviour
{
    public GameObject inputContent;
    public string inputStr;
    public static UIInput input;
    public GameObject inputClaTitle;

    bool touchEveryDay = false;
    bool touchEveryWeek = false;

    public void setContentInput()
    {//내용입력받음
        //여기서 내용글자갯수나 그런거 제한해줘야할듯
        input = inputContent.GetComponent<UIInput>();
        inputStr = input.label.text;
    }

    public void addWork()
    {
        Debug.Log("AddWork.cs의 addWork()");
        string tempDateString; //날 받아옴 string으로
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

        // 일정의 날짜가 오늘인 경우, isEventOccured=true로 해줘야함.
        // 안 그러면 이벤트 발생 초기화됨
        if (tempDateString.Equals(Calendar.currentDateString))
        {
            Schedule newWork = new Schedule(inputStr, "USER", tempDateString, true, false, inputClassfication.cla);
            GM.loadSaveManager.insertSchedule(newWork);
        }
        else
        {
            Schedule newWork = new Schedule(inputStr, "USER", tempDateString, false, false, inputClassfication.cla);
            GM.loadSaveManager.insertSchedule(newWork);
        }

        if (touchEveryDay || touchEveryWeek)
        {
            DateTime extraDate = new DateTime(Calendar.tempDate.Year, Calendar.tempDate.Month, Calendar.tempDate.Day + 1);
            string extraDateString;

            while (Calendar.tempDate.Month == extraDate.Month) //달이 같을때만 돌아야 함
            {
                if (extraDate.Month >= 10 && extraDate.Day >= 10)
                    extraDateString = extraDate.Year + "-" + extraDate.Month + "-" + extraDate.Day;
                else if (extraDate.Month >= 10)
                    extraDateString = extraDate.Year + "-" + extraDate.Month + "-0" + extraDate.Day;
                else if (extraDate.Day >= 10)
                    extraDateString = extraDate.Year + "-0" + extraDate.Month + "-" + extraDate.Day;
                else
                    extraDateString = extraDate.Year + "-0" + extraDate.Month + "-0" + extraDate.Day;

                if (touchEveryDay) { //매일 true일때
                    Schedule newWork = new Schedule(inputStr, "USER", extraDateString, false, false, inputClassfication.cla);
                    GM.loadSaveManager.insertSchedule(newWork);
                }
                else if (touchEveryWeek&&(Calendar.tempDate.DayOfWeek==extraDate.DayOfWeek)) { //요일같고 저거 true일때
                    Schedule newWork = new Schedule(inputStr, "USER", extraDateString, false, false, inputClassfication.cla);
                    GM.loadSaveManager.insertSchedule(newWork);
                }
                extraDate = extraDate.AddDays(1); //날을 1씩 더해줌
            }
        }

        // input.label.text = ""; 이거 왜 오류지...?
        GM.addWork.SetActive(false);
        GM.calendarList.SetActive(true);
    }
    public void cancel()
    {
        GM.addWork.SetActive(false);
        GM.calendarList.SetActive(true);
    }

    public void everyDayOnOff()
    {
        //이거 무슨 eventtrigger쓰려고 했는데 오류뜨넹 ㅎㅎ..ㅜㅜ
        //스크립트에서 eventtrigger를 추가해서 해야하는걸까
        //걍 오브젝트에 eventtrigger추가해서 했는데 ㅎㅎ..ㅜ
        if (!touchEveryDay)
        {
            Debug.Log("touchDown");
            touchEveryWeek = false;
        }
        else
        {
            Debug.Log("touchUp");
        }
        touchEveryDay = !touchEveryDay;
    }
    public void everyWeekOnOff()
    {
        if (!touchEveryWeek)
        {
            Debug.Log("touchDown");
            touchEveryDay = false;
        }
        else
        {
            Debug.Log("touchUp");
        }
        touchEveryWeek = !touchEveryWeek;
    }
}

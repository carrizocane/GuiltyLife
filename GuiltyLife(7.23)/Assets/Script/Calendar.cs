using System.Collections;
using UnityEngine;
using System.Globalization;
using System;
using System.Collections.Generic;

public class Calendar : MonoBehaviour
{
    public UILabel date; //위에 날짜 쓰는 아이
    public GameObject calendar;

    public static DateTime currentDate; //현재 날짜 DateTime객체
    public static DateTime tempDate;    // update Calendar에서 쓰는 Date 변수
    public static string currentDateString; //현재 날짜 string 만약 게임을 전날에 시작해서 다음날로 옮기면 어케해야할까 그거 생각하기
    public static string yesterdayDateString; //어제 날짜
    public static DateTime yesterdayDate;

    void Awake()
    {
        currentDate = DateTime.Now;      // 이 인스턴스 생길 때 딱 한번 실행이니까 여기다 씀
        tempDate = currentDate;

        //string넣어주는 if문들
        if (currentDate.Month >= 10 && currentDate.Day >= 10)
        {
            currentDateString = currentDate.Year + "-" + currentDate.Month + "-" + currentDate.Day;
        }
        else if (currentDate.Month >= 10)
        {
            currentDateString = currentDate.Year + "-" + currentDate.Month + "-0" + currentDate.Day;
        }
        else if (currentDate.Day >= 10)
        {
            currentDateString = currentDate.Year + "-0" + currentDate.Month + "-" + currentDate.Day;
        }
        else
        {
            currentDateString = currentDate.Year + "-0" + currentDate.Month + "-0" + currentDate.Day;
        }

        // 어제 날짜 설정
        // 님 코드 복붙할게여 ㄳ
        yesterdayDate = currentDate.AddDays(-1);
        if (yesterdayDate.Month >= 10 && yesterdayDate.Day >= 10)
            yesterdayDateString = yesterdayDate.Year + "-" + yesterdayDate.Month + "-" + yesterdayDate.Day;
        else if (currentDate.Month >= 10)
            yesterdayDateString = yesterdayDate.Year + "-" + yesterdayDate.Month + "-0" + yesterdayDate.Day;
        else if (currentDate.Day >= 10)
            yesterdayDateString = yesterdayDate.Year + "-0" + yesterdayDate.Month + "-" + yesterdayDate.Day;
        else
            yesterdayDateString = yesterdayDate.Year + "-0" + yesterdayDate.Month + "-0" + yesterdayDate.Day;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GM.calendar.SetActive(false);
            GM.myRoom.SetActive(true);
            GM.getInstance().slideButton.SetActive(true);
        }
    }

    void OnEnable()
    {
        //활성화시 업뎃하기
        tempDate = currentDate;
        date.text = tempDate.Year.ToString() + "년 " + tempDate.Month.ToString() + "월";
        updateCalendar();
    }

    void updateCalendar()
    {
        Debug.Log("Calendar.cs의 updateCalendar()");
        Debug.Log("캘린더 업데이트한다");
        int i = 1;
        DateTime extraDate = new DateTime(tempDate.Year,  tempDate.Month, 1); //날짜(1일 2일 ...) 넣어줄때 쓰려고
        GameObject week;
        UILabel dayNum;

        for (int j = 1; j <= 6; j++) {
            week = calendar.transform.Find("week ("+j+")").gameObject;
            for (int k = 1; k <= 7; k++)
            {
                dayNum = (UILabel)week.transform.Find("day ("+k+")").gameObject.transform.Find("Label").GetComponent("UILabel");
                dayNum.text = null;
            }
        }

        week = calendar.transform.Find("week (1)").gameObject;
        while (tempDate.Month == extraDate.Month) //달이 같을때만 돌아야 함
        {
            switch (extraDate.DayOfWeek) //숫자 넣어주기
            {
                case DayOfWeek.Sunday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (1)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Monday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (2)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (3)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (4)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (5)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (6)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        dayNum = (UILabel)week.transform.Find("day (7)").gameObject.transform.Find("Label").GetComponent("UILabel");
                        dayNum.text = extraDate.Day.ToString();
                        i++;
                        week = calendar.transform.Find("week (" + i + ")").gameObject;
                        break;
                    }
            }
            extraDate = extraDate.AddDays(1); //날을 1씩 더해줌
        }
    }

    public void gotoCalendarList()
    {
        GM.calendar.SetActive(false);
        GM.calendarList.SetActive(true);
    }

    public void gotoMyRoom()
    {
        GM.calendar.SetActive(false);
        GM.myRoom.SetActive(true);
        GM.getInstance().slideButton.SetActive(true);       //myRoom 켤 때는 얘도 켜야해염
    }

    public void preMonthButton()
    {
        tempDate =  tempDate.AddMonths(-1);
        date.text =  tempDate.Year.ToString() + "년 " +  tempDate.Month.ToString() + "월";
        updateCalendar();
    }

    public void nextMonthButton()
    {
        tempDate =  tempDate.AddMonths(1);
        date.text =  tempDate.Year.ToString() + "년 " +  tempDate.Month.ToString() + "월";
        updateCalendar();
    }  
}
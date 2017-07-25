using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchDay : MonoBehaviour {
    
    public UILabel detailLabel;
   
    public void touch()
    {
        Debug.Log("TouchDay.cs touch()");

        UILabel dayNum = (UILabel)this.transform.Find("Label").GetComponent("UILabel");
        List<Schedule> selectScheduleList;

        string tempDateString;
        if (dayNum.text == "")
        { Debug.Log("선택하면 안됨ㅇㅇ");
            return;
        }
        Calendar.tempDate = new DateTime(Calendar.tempDate.Year, Calendar.tempDate.Month, Convert.ToInt32(dayNum.text));
        //라벨값 받아와서 tempDate 바꿔줌
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

        selectScheduleList = GM.loadSaveManager.getSchedulesWithDate(tempDateString); // 선택한 날짜 넣어줌
        Debug.Log(tempDateString);

        detailLabel.text = null;
        //detailLabel에 현재 일정 넣어줌
        for (int i = 0; i < selectScheduleList.Count; i++)
        {
            Debug.Log(selectScheduleList[i].getContent());
            detailLabel.text += selectScheduleList[i].getContent() + "\n";
        }
        if (selectScheduleList.Count == 0) {
            detailLabel.text = "없다..";
        }
    }


}

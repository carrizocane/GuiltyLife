using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWork : MonoBehaviour {

    public void deleteWork()
    {
        Debug.Log("DeleteWork.cs의 deleteWork()");
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
                
        GM.loadSaveManager.deleteSchedule(GM.loadSaveManager.getSchedulesWithDate(tempDateString)[Work.deleteNum]); //삭제
        
        GM.deleteWork.SetActive(false);
        GM.calendarList.SetActive(true);
    }
    public void cancel()
    {
        Debug.Log("deleteWork cancel");
        GM.deleteWork.SetActive(false);
        GM.calendarList.SetActive(true);
    }
}

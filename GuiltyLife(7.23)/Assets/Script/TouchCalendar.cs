using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCalendar : MonoBehaviour
{
    public UIGrid grid;
    public UILabel date;
    public UIScrollView touchScroll;
    public bool enableFlag; //이거 넣어주는거 GM에서 초기화한거 순서?때매? 자꾸 오류나서 빡쳐서 만듦ㅎㅎ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GM.calendarList.SetActive(false);
            GM.calendar.SetActive(true);
        }
    }

    void Awake()
    {
        enableFlag = true;
    }

    void OnEnable()
    {
        Debug.Log("터치캘린더 활성화");
        date.text = Calendar.tempDate.Year.ToString() + "년 " + Calendar.tempDate.Month.ToString() + "월 " + Calendar.tempDate.Day.ToString() + "일";
        if (enableFlag)
        {
            enableFlag = false;
            return;
        }
        if (ReallyDone.workFlag) //만약 falg가 true인 경우 저거 실행하면 선택한 객체(this)가 소멸되어서 널오류남
        {
            Debug.Log("work check");
            checkWork();
        }
        else {
            destroyWork();
            setWork();
        }
    }

    public void destroyWork() //지금 있는 work들 한번씩 다 뿌가고 만들거라서 뿌시는 함수임
    {
        Debug.Log("TouchCalendar.cs의 destoryWork()");
        int destroyCount = grid.transform.childCount; //아가들 몇개인지 셈
        if (destroyCount != 0) //아가들 0개가 아니면 즉 존재하면
        {
            for (int i = 0; i < destroyCount; i++) //아가만큼 돌아
            {
                GameObject destroyObj = grid.transform.Find("work (" + (i + 1) + ")").gameObject; //객체 받아옴
                Destroy(destroyObj); //삭제
            }
        }
        grid.transform.DetachChildren();
    }

    public void setWork()
    {
        Debug.Log("TouchCalendar.cs의 setWork()");
        GameObject prefab = Resources.Load("Prefabs/work") as GameObject;
        // Resources/Prefabs/work.prefab 로드
        GameObject work;
        UILabel label;
        UILabel classification;

        string tempDateString; //내가 확인한? 현재?라고해야하니.. 쨋든 그 날짜 string받아와서 유저스케쥴부를때 쓸거시야 static으로 안만드는건 string보다 datetime이 수정용이
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

        UIDragScrollView setScrollView;
        List<Schedule> userScheduleList = GM.loadSaveManager.getSchedulesWithDate(tempDateString);
        
        for (int i = 0; i < userScheduleList.Count; i++)
        {
            work = MonoBehaviour.Instantiate(prefab) as GameObject;
            work.name = "work (" + (1 + i) + ")"; // name을 변경

            label = (UILabel)work.transform.Find("Label").GetComponent("UILabel"); //라벨 받아와서
            label.text = userScheduleList[i].getContent(); //내용 넣기
            classification = (UILabel)work.transform.Find("classification").GetComponent("UILabel"); //분류라벨받아와서
            switch (userScheduleList[i].getClassificationInt()) //텍스트 넣기
            {
                case 1:
                    classification.text = "과제 및 업무";
                    break;
                case 2:
                    classification.text = "약속";
                    break;
                case 3:
                    classification.text = "일상 속 할 일";
                    break;
                case 4:
                    classification.text = "작은 습관";
                    break;
                case 0:
                    classification.text = "기타";
                    break;
            }

            if (userScheduleList[i].getDone())
            { //이미 한 일정이면
                work.transform.Find("check").gameObject.SetActive(true); //체크하기
            }
            work.transform.SetParent(grid.transform);
            work.transform.localScale = new Vector3(1, 1, 1);

            setScrollView = (UIDragScrollView)work.GetComponent("UIDragScrollView");
            setScrollView.scrollView = (UIScrollView)touchScroll.GetComponent("UIScrollView");
        }
        grid.Reposition();       //이새끼 문제야..ㅜㅜ 왜 일을 안하지 execute랑 뭐가 다른지..어휴..

    }

    public void gotoAddWork()
    {
        GM.calendarList.SetActive(false);
        GM.addWork.SetActive(true);
    }

    public void gotoCalendar()
    {
        GM.calendarList.SetActive(false);
        GM.calendar.SetActive(true);
    }

    public void preDayButton()
    {
        Calendar.tempDate = Calendar.tempDate.AddDays(-1);
        date.text = Calendar.tempDate.Year.ToString() + "년 " + Calendar.tempDate.Month.ToString() + "월 " + Calendar.tempDate.Day.ToString() + "일";
        destroyWork();
        setWork();
    }

    public void nextDayButton()
    {
        Calendar.tempDate = Calendar.tempDate.AddDays(1);
        date.text = Calendar.tempDate.Year.ToString() + "년 " + Calendar.tempDate.Month.ToString() + "월 " + Calendar.tempDate.Day.ToString() + "일";
        destroyWork();
        setWork();
    }

    public void checkWork()
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

        if (ReallyDone.index == -1) //만약 -1인경우가 생기면 진짜 이상한거
            return;
        
        Debug.Log(ReallyDone.index); 
        GameObject checkWork = grid.transform.Find("work (" + (ReallyDone.index + 1) + ")").gameObject; //check할 work, +1은 1부터 이름 만들어서
        
        if (checkWork.transform.Find("check").gameObject.activeSelf) //만약 활성화 되어있으면 냅둠
            return;
        else //체크아이콘 꺼져있으면
        {
            GM.userData.setGameMoney(GM.userData.getGameMoney() + userScheduleList[ReallyDone.index].getCost()); //돈 업뎃
            userScheduleList[ReallyDone.index].setDone(true); //했다고 체크
            GM.loadSaveManager.setScheduleDone(userScheduleList[ReallyDone.index]); //db에 업뎃

            checkWork.transform.Find("check").gameObject.SetActive(true); //체크아이콘 true로 만들기
        }
        ReallyDone.workFlag = false;
    }

}

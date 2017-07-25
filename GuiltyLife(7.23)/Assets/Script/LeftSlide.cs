using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftSlide : MonoBehaviour{

    public UIGrid basicGrid;
    public UIGrid userGrid;
    public GameObject basicScroll;
    public GameObject userScroll;
    public GameObject date;
    public bool enableFlag;

    void Awake()
    {
        enableFlag = true;
    }
    void Start()
    {
        UILabel year = (UILabel)date.transform.Find("year").GetComponent("UILabel");
        UILabel day = (UILabel)date.transform.Find("day").GetComponent("UILabel");
        year.text = Calendar.currentDate.Year.ToString() + "년";
        day.text = Calendar.currentDate.Month.ToString() + "월 " + Calendar.currentDate.Day.ToString() + "일";
    }
    
    void OnEnable(){
        if (enableFlag)
        {
            enableFlag = false;
            return;
        }

        if (ReallyDone.listItemFlag) //flag가 true인 경우 삭제했다가 생성하면 오류생겨서 아예 else문에 있는거 실행안함
        {
            checkIcon();
        }
        else{
            destroyListItem();
            setListItem();
        }
    }

     public void destroyListItem() {
        //애들 다 삭제하는 거
        Debug.Log("LeftSlide.cs의 destroyListItem()");
        int basicDestroyCount = basicGrid.transform.childCount; //자식객체 몇개인지
        int userDestroyCount = userGrid.transform.childCount; //이것도 몇개인지
        if (basicDestroyCount != 0) { //아이가 없지 않으면 돌아감
            for (int i = 0; i < basicDestroyCount; i++) {
                GameObject destroyObj = basicGrid.transform.Find("basicItemList (" + (i + 1) + ")").gameObject; //받아와서
                Destroy(destroyObj); //삭제
            }
        }
        if (userDestroyCount != 0) { //아이가 없지 않으면 돌아감
            for (int i = 0; i < userDestroyCount; i++) {
                GameObject destroyObj = userGrid.transform.Find("userItemList (" + (i + 1) + ")").gameObject; //받아와서
                Destroy(destroyObj); //삭제
            }
        }

        basicGrid.transform.DetachChildren();
        userGrid.transform.DetachChildren();
    }

    public void setListItem() {

        Debug.Log("LeftSlide.cs의 setListItem()");
        GameObject prefab = Resources.Load("Prefabs/listItem") as GameObject;
        // Resources/Prefabs/listItem.prefab 로드
        GameObject listItem;
        UILabel label;
        UIDragScrollView setScrollView;

        List<Schedule> userScheduleList = GM.loadSaveManager.getSchedulesWithDate(Calendar.currentDateString);
        List<Schedule> basicScheduleList = GM.loadSaveManager.getBasicSchedules();
        //데이터 받아옴

        for (int i = 0; i < basicScheduleList.Count; i++)
        {
            listItem = Instantiate(prefab) as GameObject;
            listItem.name = "basicItemList ("+(i+1)+")"; // name을 변경

            label = (UILabel)listItem.transform.Find("Label").GetComponent("UILabel"); //라벨 찾아서
            label.text = basicScheduleList[i].getContent(); //내용넣기

            if (basicScheduleList[i].getDone()) //만약 이미 한 상태면
            {
                listItem.transform.Find("checkIcon").gameObject.SetActive(true); //체크하기
            }

            listItem.transform.SetParent(basicGrid.transform); //부모설정

            listItem.transform.localScale = new Vector3(1, 1, 1);
            setScrollView = (UIDragScrollView)listItem.GetComponent("UIDragScrollView");
            setScrollView.scrollView = (UIScrollView)basicScroll.GetComponent("UIScrollView");
        }
        basicGrid.Reposition();//위치조정

        for (int i = 0; i < userScheduleList.Count; i++)
        {
            listItem = Instantiate(prefab) as GameObject;
            listItem.name = "userItemList ("+(i+1)+")"; // name을 변경

            label = (UILabel)listItem.transform.Find("Label").GetComponent("UILabel"); //라벨 찾아서
            label.text = userScheduleList[i].getContent(); //내용넣기

            if (userScheduleList[i].getDone()) //만약 이미 한 상태면
            {
                listItem.transform.Find("checkIcon").gameObject.SetActive(true); //체크하기
            }

            listItem.transform.SetParent(userGrid.transform); //부모설정

            listItem.transform.localScale = new Vector3(1, 1, 1);
            setScrollView = (UIDragScrollView)listItem.GetComponent("UIDragScrollView");
            setScrollView.scrollView = (UIScrollView)userScroll.GetComponent("UIScrollView");
        }
        userGrid.Reposition(); //위치조정
    }

    void Update()
    {
        //  LeftSlide obj가 active일때, back버튼 누르면 LeftSlide obj 끄기
        if (Input.GetKeyDown(KeyCode.Escape) && GM.leftSlide.activeSelf)
        {
            GM.getInstance().LeftSlideOff();
        }
    }

    public void basicScrollSetActive()
    {
        if (basicScroll.activeSelf == true)
            basicScroll.SetActive(false);
        else
            basicScroll.SetActive(true);
    }

    public void userScrollSetActive()
    {
        if (userScroll.activeSelf == true)
            userScroll.SetActive(false);
        else
            userScroll.SetActive(true);
    }

    // 화면전화 효과같은거 줘야할 것 같지만, 일단 생략
    public void gotoScheduler()
    {
        GM.myRoom.SetActive(false);
        GM.leftSlide.SetActive(false);
        GM.calendar.SetActive(true);
    }


    public void checkIcon()
    {
        Debug.Log("LeftSlide.cs의 checkIcon");
        Debug.Log("일정을 수행했는지 체크한다");
        GameObject checkListItem = null; //체크할 아이템리스트
        List<Schedule> basicScheduleList = GM.loadSaveManager.getBasicSchedules(); //기본 스케쥴
        List<Schedule> userScheduleList = GM.loadSaveManager.getSchedulesWithDate(Calendar.currentDateString); //유저 스케쥴

        if (ReallyDone.index == -1) //만약 -1인경우가 생기면 진짜 이상한거
            return;

        if (ReallyDone.type.Equals("BASIC")) { //베이직타입인 경우
            Debug.Log(ReallyDone.index);
            checkListItem = basicGrid.transform.Find("basicItemList (" + (ReallyDone.index+1) + ")").gameObject; //베이직리스트 +1은 1부터 이름 만들어서
        }
        else if (ReallyDone.type.Equals("USER")) {
            Debug.Log(ReallyDone.index);
            checkListItem = userGrid.transform.Find("userItemList (" + (ReallyDone.index+1) + ")").gameObject; //유저리스트
        }

        if (checkListItem == null) //만약 null이면 저기 이프문들에 안걸린거 진짜 이상한거
        {
            Debug.Log("ERROR : doen't initialize");
            return;
        }

        if (checkListItem.transform.Find("checkIcon").gameObject.activeSelf) //만약 활성화 되어있으면 냅둠
        {
            Debug.Log("이미 활성화 됨");
            return;
        }
        else //체크아이콘 꺼져있으면
        {
            if (ReallyDone.type.Equals("BASIC")){
                GM.userData.setGameMoney(GM.userData.getGameMoney() + basicScheduleList[ReallyDone.index].getCost()); //돈업뎃
                basicScheduleList[ReallyDone.index].setDone(true); //했다고 체크
                GM.loadSaveManager.setScheduleDone(basicScheduleList[ReallyDone.index]); //db에 없데이트
            }
            else if(ReallyDone.type.Equals("USER")){
                GM.userData.setGameMoney(GM.userData.getGameMoney() + userScheduleList[ReallyDone.index].getCost()); //돈 업뎃
                userScheduleList[ReallyDone.index].setDone(true); //했다고 체크
                GM.loadSaveManager.setScheduleDone(userScheduleList[ReallyDone.index]); //db에 업뎃
            }  
            checkListItem.transform.Find("checkIcon").gameObject.SetActive(true); //체크아이콘 true로 만들기
            GM.myRoom.GetComponent<MyRoom>().UpdateStatusLabels();
        }
        ReallyDone.listItemFlag = false; //플래그 false
    }
}

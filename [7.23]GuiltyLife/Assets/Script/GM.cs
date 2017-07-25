using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour
{

    /// UI Menu & Slide & Buttons
    public static GM gm;        // singleton
    public GameObject slideButton;

    public static GameObject leftSlide;
    public static GameObject myRoom;
    public static GameObject calendar;
    public static GameObject calendarList;      //날짜 선택 시 나오는 목록 object
    public static GameObject addWork;
    public static GameObject deleteWork;
    public static GameObject reallyDone;
    public static GameObject goodEndingObj;

    //Scene
    public static GameObject adoptSceneObj;


    // Event Console
    public GameObject eventConsole;
    public UILabel eventName;
    public UILabel eventDetails;
    public UILabel eventMoneyCost;
    int basicEventCount = 0;
    bool isCEDone = false;
    //    int conditionEventCount = 0;      일단 조건부 이벤트는 1개만 해보자
    const int basicEventNumber = 3;


    //DB & Data set
    public static LoadSaveManager loadSaveManager;
    public static UserData userData;
    public static Character characterData;

    // get GM instance
    public static GM getInstance()
    {
        return gm;
    }

    /// Awake : called before Start()
    /// 주의사항! object들이 켜져있어야 static 변수 초기화 가능!!
    void Awake()
    {
        // static 변수 초기화
        leftSlide = GameObject.FindGameObjectWithTag("LeftSlide");
        myRoom = GameObject.FindGameObjectWithTag("MyRoom");
        calendar = GameObject.FindGameObjectWithTag("Calendar");
        calendarList = GameObject.FindGameObjectWithTag("CalendarList");
        addWork = GameObject.FindGameObjectWithTag("AddWork");
        deleteWork = GameObject.FindGameObjectWithTag("DeleteWork");
        reallyDone = GameObject.FindGameObjectWithTag("ReallyDone");
        goodEndingObj = GameObject.FindGameObjectWithTag("GoodEnding");
        adoptSceneObj = GameObject.FindGameObjectWithTag("Adopt");
        userData = new UserData();
        characterData = new Character();
        loadSaveManager = this.GetComponent<LoadSaveManager>();
    }

    /// Initialization
    void Start()
    {
        gm = this;
        //static 변수 초기화 이후, 쓰지 않는 UI 꺼줌
        addWork.SetActive(false);
        leftSlide.SetActive(false);
        calendar.SetActive(false);
        calendarList.SetActive(false);
        addWork.SetActive(false);
        deleteWork.SetActive(false);
        reallyDone.SetActive(false);
        goodEndingObj.SetActive(false);
        adoptSceneObj.SetActive(false);
        eventConsole.SetActive(false);      //왜지...일단 끈다

        checkToday(Calendar.currentDateString);     // 오늘의 첫 실행인지 체크하며, 첫 실행이면 여러 동작을 수행
    }

    /// Called once per frame
    void Update()
    {

    }

    /* 
     * Left Slide On & Off
     * TODO : 슬라이드 버튼 on/off -> LeftSlide 객체 on/off -> 애니메이션 및 MyRoom F.O. & F.I
     */
    public void LeftSlideOn()
    {
        Debug.Log("LeftSlide on");

        slideButton.SetActive(false);
        leftSlide.SetActive(true);

        // move left slide
        TweenPosition tw = leftSlide.AddComponent<TweenPosition>();
        tw.from.x = -640.0f;
        tw.to.x = -90.0f;
        tw.duration = 0.4f;
        tw.style = UITweener.Style.Once;
        tw.PlayForward();
    }

    public void LeftSlideOff()
    {
        Debug.Log("Left Slide Off");

        // move left slide
        TweenPosition tw = leftSlide.AddComponent<TweenPosition>();
        tw.from.x = -90.0f;
        tw.to.x = -640.0f;
        tw.duration = 0.4f;
        tw.style = UITweener.Style.Once;
        tw.onFinished.Add(new EventDelegate(this, "OnTweenFinished"));
        tw.PlayForward();
    }

    // off 트윈 종료 시 실행
    public void OnTweenFinished(UITweener tweener)
    {
        slideButton.SetActive(true);
        leftSlide.SetActive(false);
    }


    /// ☆ 오늘 첫 실행 시 진행해야하는 처리 순서          // 실행시간 : void Start()
    ///                                 단, 하루 이상 접속 하지 않은 것은 아직 고려X. 지금 설계도 충분히 머리아파
    ///                                 칭호, 아이템 컬렉션 DB는 구축하지 않아서 가장 마지막에 구현할 것
    /// 
    //  오늘의 isEventOccured 체크      (이미 실행했다면 바로 리턴, 뒤의 것들 아무것도 실행X)
    //  기본 이벤트 발생                (게임머니 감소는 그때그때 진행, UI는 바로 적용, DB에 저장은 마지막에 딱 한번)
    //  스탯 계산                       (기존 스탯, 어제 수행도를 가지고 체크)
    //  조건부 이벤트 발생              (마찬가지로 게임머니는 그때그때 감소)
    //  파산 조건 체크                  ( userData.getGameMoney() < 0 )
    /// 파산 시, bad ending, 게임 초기화, userData.killCount 증가, 새 아이 입양 진행, 칭호 체크 / 아니면 skip
    ///          => 처리 후 뒤에 것 실행X  (DateCount가 + 되면 안됨)
    //  DateCount+1                     (USER_DATA DB table의 DateCount field값이며, 현재 아이 육성 날짜 카운터)
    //  good ending 조건 체크           (DateCount >= 8)
    /// 성공 시, good ending, 게임 초기화, userData.successCount 증가, 새 아이 입양 진행, 아이템 지급, 컬렉션 추가, 칭호 체크 / 아니면 skip
    //  isEventOccured = true로 set / 모든 변동사항 DB업뎃
    //
    /// ※ 위 처리 진행함수들은 이 아래에 순서대로 작성됨



    // 오늘 접속 여부 체크 및 처리의 시작
    public void checkToday(string todayDate)
    {
        Debug.Log("[ check today called ]");
        if (loadSaveManager.isEventOccured(todayDate))      //true이면, 오늘 이미 이벤트를 봤음
            return;

        slideButton.SetActive(false);       //event 실행하는 동안 leftSlide 조작 불가
        myRoom.GetComponent<MyRoom>().dateCountUIUpdate(userData.getDateCount()+1); //dateCount UI 미리 업뎃
        calculateStatus();        // 스탯 계산

        //basic event 실행, 이벤트 콘솔 onclick에서 그 다음 이벤트 UI까지 모두 처리
        basicDailyEvent("신개념 원룸! 월세가 아닌 일세를 내야합니다! 매일매일 지출하는 기쁨!", 2000);
    }
    public void checkToday2(string todayDate)
    {
        eventConsole.SetActive(false);      // event console OFF
        slideButton.SetActive(true);

        //파산 조건 체크
        if (checkBadEnding())
        {
            // 파산하여 badEnding 실행
            badEnding();

            loadSaveManager.setScheduleEventOccured(todayDate);     //이벤트 처리 완료
            loadSaveManager.clearBasicCheck();      //체크 초기화
            loadSaveManager.saveUserData();       //변경된 userData 저장
            Debug.Log("[ Check today end with bad ending ]");
            return;         //바로 리턴하므로, userData, isEventOccured, clearBasicCheck를 여기서 따로 해주기!
        }

        // 파산 아니면 계속 진행
        userData.increaseDateCount();       //DateCount +1

        if (checkGoodEnding())
        {
            goodEnding();
        }

        loadSaveManager.setScheduleEventOccured(todayDate);     //이벤트 처리 완료
        loadSaveManager.clearBasicCheck();      //체크 초기화
        loadSaveManager.saveUserData();       //변경된 userData 저장
        Debug.Log("[ Check today end ]");
    }

    // basic 이벤트 처리
    // ex ) 일세(월세 아님)를 낸다, 식비 차감 등등
    // Params : 이벤트 정보, 게임머니 가격
    public void basicDailyEvent(string eventDetail, int eventMoney)
    {
        eventConsole.SetActive(true);
        eventName.text = "기본 이벤트 발생";
        eventDetails.text = eventDetail;
        eventMoneyCost.text = eventMoney.ToString();
        ++basicEventCount;
        userData.setGameMoney(userData.getGameMoney() - eventMoney);        //돈 감소
        myRoom.GetComponent<MyRoom>().UpdateStatusLabels();                 //UI 스탯, 돈 업데이트
    }

    // Event console의 onClick
    // 다음 basic, conditional event 순차적으로 처리
    public void onEventConsoleTouched()
    {
        if (basicEventCount >= basicEventNumber)         //basic부터 모두 봤는지 검사
        {
            // 조건 이벤트 실행
            Debug.Log("[ Conditional Event ]");
            if (isCEDone)
            {
                // conditional event 완료 시
                Debug.Log("[ Conditional Event END ]");
                eventConsole.SetActive(false);      // event console OFF
                slideButton.SetActive(true);
                checkToday2(Calendar.currentDateString);
            }
            else
            {
                //conditional event
                calculateConditionalEvent();
            }

        }
        //basic 이벤트 실행
        else
        {
            Debug.Log("[ Basic Event ]");
            switch (basicEventCount)        // 0번째는 여기서 실행하지 않음
            {
                case 1:
                    // parameter 텍스트를 랜덤하게 뽑아서 아무거나 넣는 게 더 재밌을 듯
                    basicDailyEvent("갑자기 내가 빙수가 먹고싶습니다. 빙수를 사줬습니다.", 1500);
                    break;
                case 2:
                    basicDailyEvent("음. 쓸 말이 없다. 300원 삥뜯겼습니다.", 300);
                    break;
                default:
                    Debug.Log("basic event count 에러");
                    break;
            }
        }
    }

    // userData의 status 계산 및 변경
    // 첫날 기본값 : 50, 추가 계산으로 +-
    public void calculateStatus()
    {
        // 0~100 (100 초과 시 100으로 설정)
        // DateCount : 어제까지의 날짜 수, EA PA HA는 어제까지의 스탯
        // 실행력 계산 : {(DateCount * EA) + (어제 수행한 총 일정 / 어제 전체일정)*100} / (DateCount+1)
        // 계획력 계산 : {(DateCount * PA) + (어제 유저 일정 개수 / 5)*100 } / (DateCount+1)
        // 건강력 계산 : {(DateCount * HA) + (어제 수행한 기본퀘스트/어제 전체 기본 퀘스트)*100} / (DateCount+1)

        double ea = ((userData.getDateCount() * userData.getEA()) + ((double)(loadSaveManager.getBasicDoneCount() +
                  loadSaveManager.getUserDoneCount(Calendar.yesterdayDateString))
            / (double)(loadSaveManager.getBasicAllCount() + loadSaveManager.getUserAllCount(Calendar.yesterdayDateString)) * (double)100))
                     / (double)(userData.getDateCount() + 1);

        double pa = ((userData.getDateCount() * userData.getPA()) + 
                    ((double)loadSaveManager.getUserAllCount(Calendar.yesterdayDateString)/ 5 * 100))
                     / (double)(userData.getDateCount() + 1);
        double ha = ((userData.getDateCount() * userData.getHA()) +
                    ((double)loadSaveManager.getBasicDoneCount() / loadSaveManager.getBasicAllCount() * 100))
                     / (double)(userData.getDateCount() + 1);

        userData.setEA((int)ea);
        userData.setPA((int)pa);
        userData.setHA((int)ha);

        myRoom.GetComponent<MyRoom>().UpdateStatusLabels();                 // UI status 업데이트
    }

    // conditional 이벤트 발생 여부 계산
    // 일단은 쉽게 if문으로, 딱 1개씩만 실행하도록 함 (함수 스택 순서 꼬임 방지)
    public void calculateConditionalEvent()
    {
        // 실행력이 낮은 경우
        if (userData.getEA() <= 20)
        {
            conditionalEvent("실행력이 낮습니다. 너무 막 살고 있는 모습을 보고, 아이가 불안감을 견디지 못하여 결국 탈선하고 마네요. 당신의 지갑에서 5000골드를 훔쳐갔습니다. 저런...", 5000);
        }
        // 건강이 나쁜 경우
        else if (userData.getHA() <= 25)
        {
            conditionalEvent("일찍 일어나기! 운동하기! 이건 너무 당연한 일이지만, 제대로 수행하지 못했나봅니다. 덩달아 아이도 불규칙적인 삶을 살다가, 결국 병에 걸렸네요. 치료를 위해 약값이 소모됩니다.", 3000);
        }
        // 계획력이 낮은 경우
        else if (userData.getPA() <= 30)
        {
            conditionalEvent("포부를 가지고 일정을 등록해보아요! 아이가 수동적인 성향이 강해져서, 스스로 계획을 짜는 능력이 감소했습니다. 자기주도적 학습을 잘 하지 못해서, 공부를 위해 과외 선생님을 고용합니다.", 2000);
        }
        isCEDone = true;
    }

    // conditional 이벤트 처리
    // Params : 이벤트 정보, 게임머니 가격
    public void conditionalEvent(string eventDetail, int eventMoney)
    {
        eventConsole.SetActive(true);
        eventName.text = "조건 이벤트 발생";
        eventDetails.text = eventDetail;
        eventMoneyCost.text = eventMoney.ToString();
        userData.setGameMoney(userData.getGameMoney() - eventMoney);        //돈 감소
        myRoom.GetComponent<MyRoom>().UpdateStatusLabels();                 //UI 스탯, 돈 업데이트
    }

    // 파산인지 아닌지 반환하는 함수
    public bool checkBadEnding()
    {
        return (userData.getGameMoney() < 0);
    }

    // bad ending UI 진행
    public void badEnding()
    {
        Debug.Log("[ Bad Ending... ]");
        userData.increaseKillCount();


    }

    // 캐릭터 성장 끝났는지 여부 반환하는 함수
    public bool checkGoodEnding()
    {
        return false;
    }

    // good ending UI 진행
    public void goodEnding()
    {
        Debug.Log("[ Good Ending... ]");
        userData.increaseSuccessCount();


    }

    // 게임 초기화
    // 각종 변수들을 초기화하며, badEnding과 goodEnding에서 호출
    public void clearGame()
    {

    }

    // 새 아이 입양
    // 입양을 진행하는 UI 출력 담당하며, badEnding과 goodEnding에서 호출
    public void adoptCharacter()
    {

    }


}
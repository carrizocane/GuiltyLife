using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.SqliteClient;
using System.IO;        //파일 parsing 위해서 필요
using System.Data;

public class LoadSaveManager : MonoBehaviour
{
    /// <summary>
    /// 클래스 함수 목록
    /// 
    ///         함수명                                        기능                    리턴
    /// dbSetting()                                     앱 시작 시 DB 셋팅              X
    /// 
    /// ScheduleDbParsing()                             모든 일정 리턴            List<Schedule>
    /// getSchedulesWithDate(string _date)              날짜 USER 일정 리턴       List<Schedule>
    /// getBasicSchedules()                             BASIC 일정 리턴           List<Schedule>
    /// 
    /// getBasicDoneCount()                             수행한 basic일정 개수 리턴     int
    /// getUserDoneCount(string date)                   수행한 user일정 개수 리턴      int
    /// getBasicAllCount()                              모든 basic일정 개수 리턴       int
    /// getUserAllCount()                               모든 user일정개수 리턴         int
    /// 
    /// updateSchedule(Schedule from, Schedule to)      일정 수정                       X
    /// insertSchedule(Schedule newSchedule)            새 일정 추가                    X
    /// deleteSchedule(Schedule del)                    일정 삭제                       X
    /// 
    /// clearBasicCheck()                               basic일정 체크 해제             X
    /// 
    /// setScheduleDone(Schedule s)                     일정DB의 isDone을 set           X 
    /// setScheduleEventOccured(string todayDate)       일정DB의 isEventOccured을 set   X 
    /// isEventOccured(string todayDate)                당일 이벤트 발생 여부 리턴     bool
    /// 
    /// userDataParsing()                               게임 시작시 유저 정보 초기화    X
    /// saveUserData()                                  게임 종료시 유저 정보 DB에저장  X 
    /// 
    /// characterDataParsing()                          캐릭터 정보 초기화              X
    /// 
    /// printSchedules(List<Schedule> ScheduleList)     일정 리스트 출력                X
    /// 
    /// </summary>


    public string p;          //db 파일 이름
    public string filePath;
    public string connectionString;

    void Awake()
    {
        dbSetting();
        //GM.userData 셋팅하여 정보 초기화
        userDataParsing();
        characterDataParsing();

        //check is event occured
        Debug.Log(Calendar.currentDateString);
        Debug.Log("오늘 실행을 했던가요? : " + isEventOccured(Calendar.currentDateString));
    }

    void Start()
    {
        saveUserData();
    }

    /// DB Setting
    public void dbSetting()
    {
        Debug.Log("DB setting...");

        p = "scheduler.sqlite";          //db 파일 이름
        filePath = Application.persistentDataPath + "/" + p;

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File \"" + filePath + "\"does not exist. Attempting to create from \"" + Application.dataPath + "!/assets/" + p);

            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            File.WriteAllBytes(filePath, loadDB.bytes);
        }
        connectionString = "URI=file:" + filePath;
    }

    /// db에 있는 모든 일정을 리턴
    public List<Schedule> ScheduleDbParsing()
    {
        Debug.Log("schedule parsing...");

        List<Schedule> ScheduleList = new List<Schedule>();     // 일정 리스트
        ScheduleList.Clear();

        //using 써서, 비정상적 예외 발생해도 파일이 닫힘
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())  // sql 명령 가능 
            {
                //명령어 전달
                string sqlQuery = "SELECT * FROM schedule";     //'schedule'은 db에서 일정 저장하는 table
                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader()) // schedule table의 정보들 읽어서 저장
                {
                    while (reader.Read())
                    {
                        //ScheduleList에 add
                        ScheduleList.Add(new Schedule(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetInt32(5)));
                    }

                    printSchedules(ScheduleList);   //debug

                    dbConnection.Close();
                    reader.Close();

                }   //end data reader
            }   //end db command
        }      //end db connection

        return ScheduleList;
    }

    /// 해당 날짜의 USER 일정 리스트 리턴
    /// param : 날짜 string
    public List<Schedule> getSchedulesWithDate(string _date)
    {
        Debug.Log("[ get user schedules with date called ]");
        List<Schedule> ScheduleList = new List<Schedule>();
        ScheduleList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //명령어 전달
                string sqlQuery = "SELECT * FROM schedule WHERE date='" + _date + "' AND type='USER'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduleList.Add(new Schedule(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetInt32(5)));
                    }

                    printSchedules(ScheduleList);   //debug

                    dbConnection.Close();
                    reader.Close();

                }   //end data reader
            }   //end db command
        }      //end db connection

        return ScheduleList;
    }

    /// BASIC 일정 리스트 리턴 (날짜 상관 X)
    public List<Schedule> getBasicSchedules()
    {
        Debug.Log("[ get basic schedules called ]");
        List<Schedule> ScheduleList = new List<Schedule>();
        ScheduleList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //명령어 전달
                string sqlQuery = "SELECT * FROM schedule WHERE type='BASIC'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduleList.Add(new Schedule(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetInt32(5)));
                    }

                    printSchedules(ScheduleList);   //debug

                    dbConnection.Close();
                    reader.Close();

                }   //end data reader
            }   //end db command
        }      //end db connection

        return ScheduleList;
    }

    /// 수행한 basic일정 개수 리턴
    public int getBasicDoneCount()
    {
        int basicDoneCount = 0;
  
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM schedule WHERE type='BASIC' AND isDone='true'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ++basicDoneCount;
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
        Debug.Log("[ get basic done count called ] " + basicDoneCount);
        return basicDoneCount;
    }

    /// 수행한 user일정 개수 리턴
    /// Param : date             어제 날짜를 가지고 스탯 계산에 사용
    public int getUserDoneCount(string date)
    {
        int userDoneCount = 0;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM schedule WHERE date='" + date + "' AND type='USER' AND isDone='true'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ++userDoneCount;
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
        Debug.Log("[ get user done count called ]" + userDoneCount);
        return userDoneCount;
    }

    /// 모든 basic일정 개수 리턴
    public int getBasicAllCount()
    {
        int basicAllCount = 0;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM schedule WHERE type='BASIC'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ++basicAllCount;
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
        Debug.Log("[ get basic all count called ]" + basicAllCount);
        return basicAllCount;
    }

    /// 모든 user일정개수 리턴
    /// Param : date            어제 날짜를 가지고 스탯 계산에 사용
    public int getUserAllCount(string date)
    {
        int userAllCount = 0;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM schedule WHERE date='" + date + "' AND type='USER'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ++userAllCount;
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
        Debug.Log("[ get user all count called ] " + userAllCount);
        return userAllCount;
    }

   
    ///일정 수정 (1개씩만 가능)       
    /// Param1 : 바꾸고싶은 기존 schedule 객체
    /// Param2 : 내용을 바꾼 schedule 객체
    public void updateSchedule(Schedule from, Schedule to)
    {
        Debug.Log("[ Update Schedule called ]");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // 1째 파라미터인 기존 일정을 DB에서 탐색
                // 검사 조건 : content, type, date
                string sqlQuery = "UPDATE schedule SET content='" + to.getContent() + "', type='" + to.getType() + "', date='" + to.getDate() + "', Classification=" + to.getClassificationInt() +
                " WHERE content='" + from.getContent() + "' AND type='" + from.getType() + "' AND date='" + from.getDate() + "' AND Classification=" + from.getClassificationInt() + "";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// 일정을 DB에 추가
    /// param : 추가할 schedule 객체
    public void insertSchedule(Schedule newSchedule)
    {
        Debug.Log("[ Insert Schedule called ]");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "INSERT INTO schedule VALUES ('" + newSchedule.getContent() + "', '" + newSchedule.getType()
                                + "', '" + newSchedule.getDate() + "', '" + newSchedule.getEventOccured().ToString().ToLower() + "', '" + newSchedule.getDone().ToString().ToLower() + "', " + newSchedule.getClassificationInt() + ");";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                IDataReader reader = dbCmd.ExecuteReader();
                
                dbConnection.Close();
                reader.Close();
                //end data reader
            }   //end db command
        }      //end db connection
    }

    /// 일정을 DB에서 삭제
    /// param : 삭제할 schedule 객체     // 없으면 그냥 쿼리 무시
    public void deleteSchedule(Schedule del)
    {
        Debug.Log("[Delete Schedule called]");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // flag 체크 없이, content, type, date만 체크하여 삭제
                // 조건 만족 하는 레코드 모두 삭제
                string sqlQuery = "DELETE from schedule WHERE content = '" + del.getContent() + "' AND type = '" + del.getType() + "' AND date = '" + del.getDate() + "'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// clearBasicCheck()                               basic일정 체크 해제
    /// GM checkToday에서 호출ㄱ
    public void clearBasicCheck()
    {
        Debug.Log("[Clear Bas called]");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "UPDATE schedule SET isDone = 'false'" +
                                    " WHERE type='BASIC'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// set schedule done       해당 일정을 완료한 것으로 DB 업데이트
    /// Param : Schedule 객체
    public void setScheduleDone(Schedule s)
    {
        Debug.Log("[ Set Schedule Done called ]");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // flag 체크 없이, content, type, date만 체크
                // isDone = 1로 변경 (true)
                string sqlQuery = "UPDATE schedule SET isDone='true'" +
                " WHERE content='" + s.getContent() + "' AND type='" + s.getType() + "' AND date='" + s.getDate() + "' AND Classification=" + s.getClassificationInt() + "";

                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// set schedule event occured      해당 날짜의 isEventOccured를 set
    /// Param : 날짜 string
    /// 병1신같은 문제를 찾았다. 쿼리문 2개 수행할 때 뭔가 문제가 있다.
    public void setScheduleEventOccured(string todayDate)
    {
        Debug.Log("[ Set Schedule Event Occured called ]");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // type, date만 체크
                // user의 isEventOccured = 1로 변경 (true)
                string sqlQuery = "UPDATE schedule SET isEventOccured='true'" +
                                    " WHERE date='" + todayDate + "' AND type='USER'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                IDataReader reader = dbCmd.ExecuteReader();

                // user가 없는 날도 있음. 이럴땐 basic을 가지고 처리
                // basic의 날짜가 오늘과 같으면 isEventOccurred = true
                sqlQuery = "UPDATE schedule SET date='" + todayDate + "' WHERE type='BASIC'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                reader = dbCmd.ExecuteReader();

                dbConnection.Close();
                reader.Close();  //end data reader
            }   //end db command
        }      //end db connection
    }

    /// check if event occured
    /// Param : 오늘 날짜 string
    /// 체크 방법 1 : USER의 isEventOccured로 체크
    /// 체크 방법 2 : BASIC의 날짜를 체크 (오늘 날짜이면, 오늘 실행을 했던 것)
    public bool isEventOccured(string todayDate)
    {
        bool result = false;
        Debug.Log("[ Set is Event Occured called ]");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // USER && todayDate가 있는지 체크
                string sqlQuery = "SELECT * FROM schedule WHERE date='" + todayDate + "' AND type='USER'";

                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                IDataReader reader = dbCmd.ExecuteReader();

                //case1 : USER empty
                if (!reader.Read())
                {
                    Debug.Log("     Today USER schedule EMPTY.");
                    // BASIC이 오늘 날짜인지 check
                    sqlQuery = "SELECT * FROM schedule WHERE date='" + todayDate + "' AND type='BASIC'";

                    dbCmd.CommandText = sqlQuery;
                    Debug.Log(sqlQuery);
                    reader = dbCmd.ExecuteReader();

                    if (!reader.Read())
                        result = false;     //basic이 오늘 날짜가 아님
                    else
                        result = true;      //basic이 오늘 날짜임
                }
                // case 2 : USER not empty
                else
                {
                    Debug.Log("     Today USER schedule is NOT EMPTY");
                    // USER && isEventOccured check
                    sqlQuery = "SELECT * FROM schedule WHERE date='" + todayDate + "' AND type='USER' AND isEventOccured='true'";

                    dbCmd.CommandText = sqlQuery;
                    Debug.Log(sqlQuery);
                    reader = dbCmd.ExecuteReader();

                    if (!reader.Read())
                        result = false;     //user isEventOccured = false
                    else
                        result = true;     //user isEventOccured = true
                }
                dbConnection.Close();
                reader.Close();      //end data reader
            }   //end db command
        }      //end db connection

        return result;
    }

    /// User Data Parsing
    /// Awake()에서 수행되어야함. 게임 시작 시 유저 정보를 불러옴
    public void userDataParsing()
    {
        Debug.Log("user data parsing...");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM USER_DATA";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GM.userData.setGameMoney(reader.GetInt32(0));
                        GM.userData.setDateCount(reader.GetInt32(1));
                        GM.userData.setEA(reader.GetInt32(2));
                        GM.userData.setPA(reader.GetInt32(3));
                        GM.userData.setHA(reader.GetInt32(4));
                        GM.userData.setIQ(reader.GetInt32(5));
                        GM.userData.setFA(reader.GetInt32(6));
                        GM.userData.setKillCount(reader.GetInt32(7));
                        GM.userData.setSuccessCount(reader.GetInt32(8));
                        GM.userData.setCharacterId(reader.GetInt32(9));
                        GM.userData.setStartDate(reader.GetString(10));
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// User Data Update
    /// 이벤트 수행 이후, 게임 종료 시 호출
    public void saveUserData()
    {
        Debug.Log("[ Save user data called ]");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // USER_DATA의 레코드는 1개니까 조건문 안써도 될 듯?
                string sqlQuery = "UPDATE USER_DATA SET GameMoney=" + GM.userData.getGameMoney() + ", DateCount=" + GM.userData.getDateCount()
                    + ", EA=" + GM.userData.getEA() + ", PA=" + GM.userData.getPA() + ", HA=" + GM.userData.getHA()
                    + ", IQ=" + GM.userData.getIQ() + ", FA=" + GM.userData.getFA()
                    + ", KillCount=" + GM.userData.getKillCount() + ", SuccessCount=" + GM.userData.getSuccessCount()
                    + ", CharacterId=" + GM.userData.getCharacterId() + ", StartDate='" + GM.userData.getStartDate() + "'";
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }

    /// 캐릭터 데이터 가져오기
    /// ID값의 캐릭터 정보 하나만 가져옴
    public void characterDataParsing()
    {
        // GM.userData.getCharacterId() 로 가져오기
        Debug.Log("character data parsing...");
        string[] msgData = new string[5];

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM CHARACTER WHERE ID=" + GM.userData.getCharacterId();
                dbCmd.CommandText = sqlQuery;
                Debug.Log(sqlQuery);
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GM.characterData.setId(reader.GetInt32(0));
                        GM.characterData.setName(reader.GetString(1));
                        GM.characterData.setcharacterIQ(reader.GetInt32(2));
                        GM.characterData.setCharacterFA(reader.GetInt32(3));
                        msgData[0] = reader.GetString(4);
                        msgData[1] = reader.GetString(5);
                        msgData[2] = reader.GetString(6);
                        msgData[3] = reader.GetString(7);
                        msgData[4] = reader.GetString(8);
                        GM.characterData.setMainMessages(msgData);

                        GM.userData.setIQ(GM.characterData.getCharacterIQ());       //IQ, FA를 해당 캐릭터가 가진 값으로 대입
                        GM.userData.setFA(GM.characterData.getCharacterFA());
                    }
                    dbConnection.Close();
                    reader.Close();
                }   //end data reader
            }   //end db command
        }      //end db connection
    }


    /// 디버그용
    /// 일정 리스트를 콘솔에 출력
    public void printSchedules(List<Schedule> ScheduleList)
    {
        if (ScheduleList.Count == 0)
        {
            Debug.Log("[ printSchedules called ] empty");
        }
        Debug.Log("[ printSchedules called ] item 개수 = " + ScheduleList.Count);
        // 값 제대로 들어갔나 확인할 때 쓰세염!
        for (int i = 0; i < ScheduleList.Count; i++)
        {
            Debug.Log(i + "째 data : " + ScheduleList[i].print());
        }
    }

}
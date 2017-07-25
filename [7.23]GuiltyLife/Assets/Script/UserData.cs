using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// User Data 모두 DB에서 불러와서 Awake()에서 초기화해줘야함
    /// USER_DATA 테이블 사용
    /// 게임 실행시, LoadSaveManager에서 GM의 static UserData 변수 초기화 후, Start()에서 Character instantiate() ㄱㄱ
    /// 주의!
    /// 이 클래스의 정보 변경 시, 꼭 DB도 같이 업데이트할 것
public class UserData {

    private int gameMoney = 0;      // 게임 머니
    private int dateCount = 1;           // 현재 아이 시작으로부터 몇번째 날인지 정보
                                        // 오늘 날짜 - startDate + 1이 dateCount

    // User status
    private int executiveAbility = 0;   // 실행력   EA
    private int planningAbility = 0;    // 계획력   PA
    private int health = 0;             // 건강     HA
    private int IQ = 0;                 // IQ       IQ
    private int faceAbility = 0;        // 얼굴력   FA

    // User play record 
    private int killCount = 0;      // 지금까지 죽인 아이 카운터
    private int successCount = 0;   // 지금까지 성공한 아이 카운터
    private int characterId = 0;        // 1부터 시작, DB CharacterData 테이블의 Id값        // 현재 키우는 아이 정보 

    private string startDate = "EMPTY";    //현재 아이 시작 날짜, YYYY-MM-DD 형식

    // Constructor
    public UserData()
    {
        setGameMoney(0);
        setDateCount(1);

        setEA(0);
        setPA(0);
        setHA(0);
        setIQ(0);
        setFA(0);

        setKillCount(0);
        setSuccessCount(0);
        setCharacterId(0);
    }

    public UserData(int gameMoney, int dateCount, int EA, int PA, int HA, int IQ, int FA, int killCount, int successCount, int characterId, string startDate)
    {
        setGameMoney(gameMoney);
        setDateCount(dateCount);

        setEA(EA);
        setPA(PA);
        setHA(HA);
        setIQ(IQ);
        setFA(FA);

        setKillCount(killCount);
        setSuccessCount(successCount);
        setCharacterId(characterId);
        setStartDate(startDate);
    }

    // setters & getters
    public void setGameMoney(int gameMoney)
    {
        this.gameMoney = gameMoney;
    }
    public int getGameMoney()
    {
        return gameMoney;
    }

    public void setDateCount(int dateCount)
    {
        this.dateCount = dateCount;
    }
    public void increaseDateCount()
    {
        ++dateCount;
    }
    public int getDateCount()
    {
        return dateCount;
    }

    public void setEA(int EA)
    {
        executiveAbility = EA;
    }
    public int getEA()
    {
        return executiveAbility;
    }

    public void setPA(int PA)
    {
        planningAbility = PA;
    }
    public int getPA()
    {
        return planningAbility;
    }

    public void setHA(int HA)
    {
        health = HA;
    }
    public int getHA()
    {
        return health;
    }

    public void setIQ(int IQ)
    {
        this.IQ = IQ;
    }
    public int getIQ()
    {
        return IQ;
    }

    public void setFA(int FA) {
        faceAbility = FA;
    }
    public int getFA()
    {
        return faceAbility;
    }

    public void setKillCount(int killCount)
    {
        this.killCount = killCount;
    }
    public void increaseKillCount()
    {
        ++killCount;
    }
    public int getKillCount()
    {
        return killCount;
    }

    public void setSuccessCount(int successCount)
    {
        this.successCount = successCount;
    }
    public void increaseSuccessCount()
    {
        ++successCount;
    }
    public int getSuccessCount()
    {
        return successCount;
    }

    public void setCharacterId(int characterId)
    {
        this.characterId = characterId;
    }
    public int getCharacterId()
    {
        return characterId;
    }

    public void setStartDate(string startDate)
    {
        this.startDate = startDate;
    }
    public string getStartDate()
    {
        return startDate;
    }
}

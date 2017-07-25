using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 생각해보니까, BASIC 일정들은 날짜 필요 없는 것 같음
/// 그러면 (날짜 상관 없이) BASIC만 리턴하는 함수는 LoadSaveManager에 따로 만들겠음! 
/// 
/// </summary>
public class Schedule
{
    // 기타, 작업, 약속, 일상, 습관
    public enum Classification { ETC = 0, WORK, APPOINTMENT, DAILY_LIFE, HABIT }     // user quest 분류
    public static readonly int[] userQuestCost = { 200, 500, 300, 250, 200 };       //user quest 가격
    public static readonly int basicQuestCost = 1000;           // basic quest 가격

    private string content = "";    // 일정
    private string type = "";       // USER or BASIC
    private string date = "YYYY-MM-DD";     //날짜        //아직 안쓰고 테스트할거라..ㅇㅇ
    private bool isEventOccured;    //이 날에 이벤트를 봤는지(당일 앱 처음 실행 시 발생함)
    private bool isDone;            //해당 일정이 수행되었는지
    public Classification classification;       //분류

    //constructor
    public Schedule()
    {
        setContent("TODO");
        setType("USER");
        setDate("YYYY-MM-DD");
        setEventOccured(false);
        setDone(false);
        setClassificationInt(1);
    }

    public Schedule(string content, string type, string date, bool isEventOccured, bool isDone, int classification)
    {
        setContent(content);
        setType(type);
        setDate(date);
        setEventOccured(isEventOccured);
        setDone(isDone);
        setClassificationInt(classification);
    }

    //setters & getters
    public void setContent(string content)
    {
        this.content = content;
    }

    public string getContent()
    {
        return content;
    }

    public void setType(string type)
    {
        if (type.Equals("USER") || type.Equals("BASIC"))
            this.type = type;
        else
            Debug.Log("setType() error : " + type);
    }

    public string getType()
    {
        return type;
    }

    public void setDate(string date)
    {
        this.date = date;
    }

    public string getDate()
    {
        return date;
    }

    public void setEventOccured(bool isEventOccured)
    {
        this.isEventOccured = isEventOccured;
    }

    public bool getEventOccured()
    {
        return isEventOccured;
    }

    public void setDone(bool isDone)
    {
        this.isDone = isDone;
    }

    public bool getDone()
    {
        return isDone;
    }

    public void setClassificationInt(int c)
    {
        classification = (Classification)c;
    }
    public int getClassificationInt()
    {
        return (int)classification;
    }

    //get cost
    public int getCost()
    {
        if (getType().Equals("BASIC"))
            return basicQuestCost;
        else        //USER type인 경우
            return userQuestCost[getClassificationInt()];
    }

    //디버그용
    public string print()
    {
        return "" + this.getContent() + ", " + this.getType() + ", " + this.getDate() + ", " + this.getEventOccured() + ", " + this.getDone() + ", " + this.classification.ToString();
    }

}

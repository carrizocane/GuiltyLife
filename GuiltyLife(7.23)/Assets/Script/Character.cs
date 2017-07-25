using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DB의 Character 테이블에 여러 종류 아이들 정보가 저장됨
/// 게임 시작 시, userData의 characterId를 알아오고(여기까진 Awake()에서 처리함)
/// id에 따라서 DB에서 아이 정보 불러와서, GM의 아이 변수 초기화 후
/// Start()에서 아이 오브젝트 생성해서 마이룸에 배치
/// 
/// 이미지 정보는 굳이 저장 안해도 괜춘할 듯?
/// 
/// </summary>
public class Character{

    //db의 CHARACTER 테이블에서 불러와 저장할 정보
    private int characterId = 0;      //1부터
    private string name;
    private int characterIQ;
    private int characterFA;
    private string[] mainMessages;       //5개

    //Constructor
    public Character()
    {
        setId(0);
        setName("코코아맛 까까");
        setcharacterIQ(0);
        setCharacterFA(0);
        setMainMessages(null);
    }

    public Character(int id, string name, int IQ, int FA, string[] mainMessages)
    {
        setId(id);
        setName(name);
        setcharacterIQ(IQ);
        setCharacterFA(FA);
        setMainMessages(mainMessages);
    }

    //setters & getters
    public void setId(int id)
    {
        characterId = id;
    }
    public int getId()
    {
        return characterId;
    }

    public void setcharacterIQ(int IQ)
    {
        characterIQ = IQ;
    }
    public int getCharacterIQ()
    {
        return characterIQ;
    }

    public void setCharacterFA(int FA)
    {
        characterFA = FA;
    }
    public int getCharacterFA()
    {
        return characterFA;
    }

    public void setName(string name)
    {
        this.name = name;
    }
    public string getName()
    {
        return name;
    }

    public void setMainMessages(string[] mainMessage)
    {
        this.mainMessages = mainMessage;
    }
    public string[] getMainMessages()
    {
        return mainMessages;
    }
}

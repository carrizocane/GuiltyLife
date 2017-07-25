using UnityEngine;
using System.Collections;

public class MyRoom : MonoBehaviour {
    
    // menus
    public UISprite settingButton;
    public UISprite collectionButton;
    public UISprite medalButton;
    public GameObject statusSet;

    // status
    public bool isStatusOn = false;
    public UILabel[] statusLabel = new UILabel[5];
    public UILabel moneyLabel;
    public UILabel dateCount;

    //variables
    public UILabel characterLabel;
    public static int characterMsgIndex = 0;


    // Use this for initialization
    void Start () {
        characterLabel.text = GM.characterData.getMainMessages()[0];
        UpdateStatusLabels();
        dateCount.text = "Day " + GM.userData.getDateCount().ToString();
 
    }

    /* TODO:
     * LeftSlide obj가 inactive 상태이면, 터치드래그 했을 때, GM의 LeftSlideOn() 호출 해야함
     */
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !GM.leftSlide.activeSelf)
        {
            Application.Quit();
        }
    }

    void OnClick()
    {
        // LeftSlide obj가 active 상태이며, 이 바깥부분 눌렀을 때 => LeftSlide OFF
        if (GM.leftSlide.activeSelf)
        {
            GM.getInstance().LeftSlideOff();
        }
    }

    // 능력치, 돈 Label 업데이트
    public void UpdateStatusLabels()
    {
        statusLabel[0].text = "실 행 력 : " + GM.userData.getEA().ToString();      //EA
        statusLabel[1].text = "계 획 력 : " + GM.userData.getPA().ToString();      //PA
        statusLabel[2].text = "건 강 : " + GM.userData.getHA().ToString();      //HA
        statusLabel[3].text = "I  Q : " + GM.userData.getIQ().ToString();      //IQ
        statusLabel[4].text = "얼 굴 력 : " + GM.userData.getFA().ToString();      //FA

        moneyLabel.text = GM.userData.getGameMoney().ToString();
    }

    public void statusOnOff()
    {
        if (isStatusOn == true)
            statusSet.SetActive(false);
        else
            statusSet.SetActive(true);
        isStatusOn = !isStatusOn;
    }

    public void characterMsgChange()
    {
        if(characterMsgIndex != 4)
            characterLabel.text = GM.characterData.getMainMessages()[++characterMsgIndex];
        else
        {
            characterLabel.text = GM.characterData.getMainMessages()[0];
            characterMsgIndex = 0;
        }
            
    }

    public void dateCountUIUpdate(int day)
    {
        dateCount.text = "Day " + day;
    }

}

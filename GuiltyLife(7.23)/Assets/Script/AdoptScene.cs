using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdoptScene : MonoBehaviour {

    //scene obj
    public GameObject text1;        // 딩동
    public GameObject text2;        // 택배 왔습니다
    public GameObject balloon;      // ? 뭐지
    public GameObject door;         //문
    public UISprite door_shadow;    //문 그림자
    public UISprite box;            // 아기가 담긴 박스
    public GameObject surprise;     // 앗!

    bool isTouchActivated = false;

    enum SceneOrder { NONE=0, TEXT1, TEXT2, BALLOON, OPEN_DOOR, BOX, SURPRISE }
    SceneOrder order = SceneOrder.NONE;

    // 까만 화면이 밝아지면 시작
    public void adoptStart()
    {
        Debug.Log("입양씬 활성화");
        isTouchActivated = true;
    }

    //터치할때마다 진행
    public void bgTouched()
    {
        if (isTouchActivated == false)
            return;

        //터치 활성화 된 경우, 터치 시 다음 장면 실행
        ++order;
        Debug.Log("[ BG Touched 실행 ] order : " + order);

        // 딩동-
        if(order == SceneOrder.TEXT1)
        {
            text1.SetActive(true);
        }
        //택배왔습니다
        else if(order == SceneOrder.TEXT2)
        {
            text2.SetActive(true);
        }
        //? 뭐지
        else if(order == SceneOrder.BALLOON)
        {
            text1.SetActive(false);
            text2.SetActive(false);
            balloon.SetActive(true);
        }
        // 문열리는 트윈
        else if (order == SceneOrder.OPEN_DOOR)
        {
            var rotatePos = Quaternion.Euler(0f, 65f, 0f);
            TweenRotation tw = TweenRotation.Begin(door, 1.0f, rotatePos);
            tw.PlayForward();

        }
        // 상자 나타남
        else if(order == SceneOrder.BOX)
        {
            balloon.SetActive(false);
            door.SetActive(false);
            door_shadow.enabled = false;
            box.enabled = true;
        }
        // 앗!
        else if(order == SceneOrder.SURPRISE)
        {
            surprise.SetActive(true);
        }


    }
}

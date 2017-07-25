using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ex : MonoBehaviour {

    public GameObject adoptScene;

    public void me() {
        GM.myRoom.SetActive(false);
        GM.goodEndingObj.SetActive(true);

    }

    // 나도 이거 쓸랭ㅋㅋㅋ
    public void adoptSceneTest()
    {
        GM.myRoom.SetActive(false);
        GM.getInstance().slideButton.SetActive(false);
        adoptScene.SetActive(true);
    }

}

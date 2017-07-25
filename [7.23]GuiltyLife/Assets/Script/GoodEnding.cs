using UnityEngine;
using System.Collections;

public class GoodEnding : MonoBehaviour
{
    public GameObject goodEnding;
    float m_fDuration = 3f;
    bool flag = false;

    void OnEnable()
    {
        Debug.Log("활성화됨");

        goodEnding.transform.Find("PostBox&Letter").gameObject.GetComponent<UIButton>().isEnabled = false;
        if (flag)
            fadeIn();

        flag = true;

    }

    public void fadeIn()
    {
        Debug.Log("페이드인 실행");

        StartCoroutine(FadeIn(goodEnding.transform.Find("Labels").transform.Find("Label (1)").gameObject));
        StartCoroutine(FadeIn(goodEnding.transform.Find("Labels").transform.Find("Label (2)").gameObject));
        StartCoroutine(FadeIn(goodEnding.transform.Find("Black").gameObject));

    }

   public void setEnable() {
        Debug.Log("트윈 끝나면 실행");
        goodEnding.transform.Find("PostBox&Letter").gameObject.GetComponent<UIButton>().isEnabled = true; //이게 저들의 tween이 끝나면 실행되었으면 좋겠어ㅜ
    }

    IEnumerator FadeOut(GameObject obj)
    {
        Debug.Log("페이드아웃 코루틴");
        //FadeOut 

        TweenAlpha ta = TweenAlpha.Begin(obj, m_fDuration, 1f);
        ta.eventReceiver = goodEnding;
         ta.callWhenFinished = "setEnable"; 
        ta.delay = 3f;
        yield return new WaitForSeconds(m_fDuration);

    }
    IEnumerator FadeIn(GameObject obj)
    {
        Debug.Log("페이드인 코루틴");
        //FadeIn 
        TweenAlpha ta = TweenAlpha.Begin(obj, m_fDuration, 0f);
        ta.delay = 2f;
        yield return new WaitForSeconds(m_fDuration);

    }

    public void openLetter()
    {
        goodEnding.transform.Find("Letter").gameObject.SetActive(true);
        goodEnding.transform.Find("PostBox").gameObject.SetActive(true);
        goodEnding.transform.Find("PostBox&Letter").gameObject.SetActive(false);
    }

    public void closeLetter()
    {
        goodEnding.transform.Find("Letter").gameObject.SetActive(false);
        goodEnding.transform.Find("Gift").gameObject.SetActive(true);
        tweenGift();
    }

    void tweenGift()
    {
        TweenScale tw = goodEnding.transform.Find("Gift").gameObject.AddComponent<TweenScale>();
        tw.from.x = 0.9f;
        tw.to.x = 1.1f;
        tw.from.y = 0.9f;
        tw.to.y = 1.1f;
        tw.duration = 1f;
        tw.style = UITweener.Style.Loop;
        tw.PlayForward();
    }

    public void touchGift()
    {
        goodEnding.transform.Find("Gift").gameObject.SetActive(false);
        goodEnding.transform.Find("White").gameObject.SetActive(true);
        //얘는 트윈 왜 지멋대로 켜짐..?ㅡ
        //트윈끝나면 이 밑의 아이 틀고싶음
        goodEnding.transform.Find("GetItem").gameObject.SetActive(true);

    }

    public void touchGetItem()
    {

        goodEnding.transform.Find("GetItem").gameObject.SetActive(false);
        goodEnding.transform.Find("White").gameObject.SetActive(false);

        StartCoroutine(FadeOut(goodEnding.transform.Find("Black").gameObject));

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_System : MonoBehaviour
{
    private bool isUIOn = false;

    private Animator uiAnim;

    public UI_CraftSystem craftSystem;

    public GameObject craftBtn_On;
    public GameObject blankBtn_0_On;
    public GameObject blankBtn_1_On;

    public GameObject craftObj;
    public GameObject blankObj_0;
    public GameObject blankObj_1;

    public RectTransform ui_Rect_On;
    public RectTransform ui_Rect_Off;

    private void Start()
    {
        uiAnim = GetComponent<Animator>();
    }

    /// <summary>
    ///  시스템 UI 버튼 다 끄기
    /// </summary>
    private void SetActiveFalse()
    {
        craftBtn_On.SetActive(false);
        blankBtn_0_On.SetActive(false);
        blankBtn_1_On.SetActive(false);

        //craftObj.SetActive(false);
        //blankObj_0.SetActive(false);
        //blankObj_1.SetActive(false);
    }

    /// <summary>
    /// 크래프팅 버튼 클릭했을 때
    /// </summary>
    public void OnClick_StartCraftSystem()
    {
        OnUIAnimation();
        
        SetActiveFalse();

        craftBtn_On.SetActive(isUIOn);
        craftSystem.OnCraft();

        //craftObj.SetActive(true);
    }

    /// <summary>
    /// 올라가거나 내려가는 애니메이션 추가
    /// </summary>
    private void OnUIAnimation()
    { 
        if(isUIOn == false)
        {
            uiAnim.SetTrigger("UIOn");
            isUIOn = true;
        }
        else
        {
            uiAnim.SetTrigger("UIOff");
            isUIOn = false;
        }
    }

    /*// UI 바로 켜기
    public void ShowUIImmediately()
    {
        isUIOn = true;
        uiAnim.SetTrigger("UIOnImmediate");
    }*/

    // UI 바로 끄기 
    public void HideUIImmediately()
    {
        SetActiveFalse();   // 다 끄고
        isUIOn = false; // 현재 UI 상태 체크하는 식별자
        uiAnim.SetTrigger("UIOffImmediate");
    }
}

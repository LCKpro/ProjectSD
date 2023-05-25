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

    private void Start()
    {
        uiAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// 크래프팅 버튼 클릭했을 때
    /// </summary>
    public void OnClick_StartCraftSystem()
    {
        OnUIAnimation();

        if (craftBtn_On.activeSelf == true)
        {
            craftBtn_On.SetActive(false);
            craftObj.SetActive(false);

            return;
        }

        craftSystem.OnCraft();

        craftBtn_On.SetActive(true);
        blankBtn_0_On.SetActive(false);
        blankBtn_1_On.SetActive(false);

        craftObj.SetActive(true);
        blankObj_0.SetActive(false);
        blankObj_1.SetActive(false);
    }

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
}

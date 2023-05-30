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
    ///  �ý��� UI ��ư �� ����
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
    /// ũ������ ��ư Ŭ������ ��
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
    /// �ö󰡰ų� �������� �ִϸ��̼� �߰�
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

    /*// UI �ٷ� �ѱ�
    public void ShowUIImmediately()
    {
        isUIOn = true;
        uiAnim.SetTrigger("UIOnImmediate");
    }*/

    // UI �ٷ� ���� 
    public void HideUIImmediately()
    {
        SetActiveFalse();   // �� ����
        isUIOn = false; // ���� UI ���� üũ�ϴ� �ĺ���
        uiAnim.SetTrigger("UIOffImmediate");
    }
}

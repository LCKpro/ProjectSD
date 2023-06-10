using System;
using UnityEngine;
using UniRx;
using TMPro;


public partial class AI_Structure
{
    public Animator repairAnim;
    public TextMeshProUGUI repairTxt;

    public GameObject btn_Repair;

    /// <summary>
    /// ���� ü�� �ۼ�Ʈ�� �����ִ� UI ����/����
    /// </summary>
    public void ShowHealthUI()
    {

    }

    private void HideHelathUI()
    {

    }

    public void RepairStructure(float repairTime, Player_0001 player)
    {
        StopRepair();
        Repair(repairTime);
        _repairController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if (healthValue >= maxHealthValue)
                {
                    healthValue = maxHealthValue;
                    StopRepair();
                    player.ChangeStateToIdle();
                }
                else
                {
                    Repair(repairTime);
                }
            });
    }

    private void Repair(float repairTime)
    {
        float gainHp = maxHealthValue / repairTime;
        //repairTxt.text = "+" + gainHp.ToString();
        //repairAnim.SetTrigger("Repair");    // ȸ���� �Ǿ��� �� ��� UI Animation
        healthValue += (gainHp);

        if (healthValue > maxHealthValue)
        {
            healthValue = maxHealthValue;
        }
    }

    public void StopRepair()
    {
        _repairController.Dispose();
        _repairController = Disposable.Empty;
    }

    public float GetStructureHpRate()
    {
        return healthValue / maxHealthValue;
    }

    private bool isClick = false;
    /// <summary>
    /// �ϴ� ��ų ��� Ȯ�� �� �ǹ��� Ŭ������ �� -> ������ư�� �����ϵ���
    /// </summary>
    public void OnClick_Skill0001_CheckStructure()
    {
        if (isClick == false)    // ó�� Ŭ���ѰŸ� 
        {
            GamePlay.Instance.skillManager.SetTargetStructure(this);
        }
        else
        {
            GamePlay.Instance.skillManager.DeActive_Skill0001();
        }
    }


    private void OnMouseDown()
    {
        Debug.Log("Ŭ�� ����");

        if (GamePlay.Instance.gameStateManager.CheckSkillStateType(GameDefine.SkillStateType.Player0001Skill) == false)
        {
            return;
        }

        OnClick_Skill0001_CheckStructure();
    }
}

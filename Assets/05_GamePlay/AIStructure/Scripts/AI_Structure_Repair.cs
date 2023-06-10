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
    /// 현재 체력 퍼센트를 보여주는 UI 등장/숨김
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
        //repairAnim.SetTrigger("Repair");    // 회복이 되었을 때 띄울 UI Animation
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
    /// 일단 스킬 사용 확인 후 건물을 클릭했을 때 -> 수리버튼이 등장하도록
    /// </summary>
    public void OnClick_Skill0001_CheckStructure()
    {
        if (isClick == false)    // 처음 클릭한거면 
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
        Debug.Log("클릭 감지");

        if (GamePlay.Instance.gameStateManager.CheckSkillStateType(GameDefine.SkillStateType.Player0001Skill) == false)
        {
            return;
        }

        OnClick_Skill0001_CheckStructure();
    }
}

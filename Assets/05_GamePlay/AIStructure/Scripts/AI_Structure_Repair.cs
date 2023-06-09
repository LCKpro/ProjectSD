using System;
using UnityEngine;
using UniRx;
using TMPro;


public partial class AI_Structure
{
    public Animator repairAnim;
    public TextMeshProUGUI repairTxt;

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

}

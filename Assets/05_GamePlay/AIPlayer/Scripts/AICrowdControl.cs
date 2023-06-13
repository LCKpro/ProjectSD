using UnityEngine;
using UniRx;
using System;

public partial class AIPlayer
{
    private IDisposable _enableToMoveTimer = Disposable.Empty;
    private IDisposable _ableToMoveTimer = Disposable.Empty;


    private void StopCrowdControl_EnableToMove()
    {
        _enableToMoveTimer.Dispose();
        _enableToMoveTimer = Disposable.Empty;
    }

    private void StopCrowdControl_AbleToMove()
    {
        _ableToMoveTimer.Dispose();
        _ableToMoveTimer = Disposable.Empty;
    }

    /*public override void TakeCrowdControl(GameDefine.CCType ccType, float duration, float percent = 0.01f)
    {
        Debug.Log(ccType + "�� CC�⸦ ����");

        if (ccType == GameDefine.CCType.Slow)
        {
            SlowStart(duration, percent);
        }
        else if (ccType == GameDefine.CCType.Stun)
        {
            StunStart(duration);
        }
        else
        {

        }
    }*/

    protected override void SlowStart(float duration, float percent)
    {
        Debug.Log("��ȭ : " + duration + "��, " + percent + "%");
        StopCrowdControl_AbleToMove();
        finalMoveSpeed = moveSpeed * percent;
        _ableToMoveTimer = Observable.Interval(TimeSpan.FromSeconds(duration)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                finalMoveSpeed = moveSpeed;
            });
    }

    protected override void StunStart(float duration)
    {
        Debug.Log("���� : " + duration + "��");
        StopAIController();
        StopAlMove();
        StopCrowdControl_EnableToMove();
        anim.SetInteger("animation", 19);    // ���ڸ����� cc�ɷ��� �� �ִϸ��̼�(IdleA)
        _enableToMoveTimer = Observable.Interval(TimeSpan.FromSeconds(duration)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                //AIControllerStart();
            });
    }
}

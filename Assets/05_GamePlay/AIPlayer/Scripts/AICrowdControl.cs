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
        Debug.Log(ccType + "의 CC기를 받음");

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
        Debug.Log("둔화 : " + duration + "초, " + percent + "%");
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
        Debug.Log("기절 : " + duration + "초");
        StopAIController();
        StopAlMove();
        StopCrowdControl_EnableToMove();
        anim.SetInteger("animation", 19);    // 제자리에서 cc걸렸을 때 애니메이션(IdleA)
        _enableToMoveTimer = Observable.Interval(TimeSpan.FromSeconds(duration)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                //AIControllerStart();
            });
    }
}

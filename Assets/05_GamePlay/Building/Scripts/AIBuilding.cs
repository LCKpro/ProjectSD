using UniRx;
using System;
using UnityEngine;

public class AIBuilding : Stat
{
    public GameDefine.CraftType craftType;

    private IDisposable _timer = Disposable.Empty;

    private void Start()
    {
        var pos = transform.position;
        pos.y = -2;
        transform.position = pos;

        BuildingRaise();
    }

    private void BuildingRaise()
    {
        StopRaise();
        _timer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                var time = Time.deltaTime;
                if(transform.position.y >= 0)
                {
                    StopRaise();
                }
                else
                {
                    var pos = transform.position;
                    pos.y += time;
                    transform.position = pos;
                }
            });
    }

    private void StopRaise()
    {
        _timer.Dispose();
        _timer = Disposable.Empty;
    }


    protected override void Die()
    {
        transform.gameObject.SetActive(false);
    }
}

using UniRx;
using System;
using UnityEngine;

public partial class AI_Structure
{
    public GameDefine.CraftType craftType;
    public GameObject spawnEffect;

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
        spawnEffect.SetActive(false);
        StopRaise();
        bool isShake = false;
        float shakePower = 0.015f;
        var originPos = transform.position;
        _timer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                var time = Time.deltaTime;
                if (transform.position.y >= 0)
                {
                    StopRaise();
                    spawnEffect.SetActive(true);
                }
                else
                {
                    var pos = transform.position;

                    if(isShake == true)
                    {
                        pos.x = originPos.x - shakePower;
                        pos.z = originPos.z - shakePower;
                        isShake = false;
                    }
                    else
                    {
                        pos.x = originPos.x + shakePower;
                        pos.z = originPos.z + shakePower;
                        isShake = true;
                    }

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

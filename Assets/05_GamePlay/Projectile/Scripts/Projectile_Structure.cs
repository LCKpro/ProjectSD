using System;
using UnityEngine;
using UniRx;

public class Projectile_Structure : MonoBehaviour
{
    public float power;

    private IDisposable _atkController = Disposable.Empty;
    private Rigidbody _rigid;
    private AI_Structure ai_Structure;

    // 공격하는데 성공했으면 타이머 끄기
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;
    }

    public void ReadyAndShot(AI_Structure structure, GameObject target, float speed)
    {
        ai_Structure = structure;
        _rigid = GetComponent<Rigidbody>();

        _rigid.AddForce(target.transform.position * power, ForceMode.Impulse);

        _atkController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                this.transform.position = Vector3.Slerp(transform.position, target.transform.position, Time.deltaTime);
            });
    }

    public void RangeAttackStart(Collider other)
    {
        _atkController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.DealDamage(other.gameObject);
            GamePlay.Instance.spawnManager.ReturnProjectilePool(this.transform);
            StopAttack();
        }
    }
}

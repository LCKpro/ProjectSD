using System;
using UnityEngine;
using UniRx;

public class Projectile_Balista : MonoBehaviour
{
    public float power;
    public string idName;

    private IDisposable _atkController = Disposable.Empty;
    private AI_Structure ai_Structure;

    // 공격하는데 성공했으면 타이머 끄기
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;
    }

    public void ReadyAndShot(AI_Structure structure, GameObject target)
    {
        ai_Structure = structure;
        Vector3 pos = new Vector3(target.transform.position.x, 2f, target.transform.position.z);
        Debug.Log(pos);
        _atkController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, 0.3f);
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.DealDamage(other.gameObject);
            GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
            StopAttack();
        }
    }
}

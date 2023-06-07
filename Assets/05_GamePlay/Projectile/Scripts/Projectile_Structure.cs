using System;
using UnityEngine;
using UniRx;

public class Projectile_Structure : MonoBehaviour
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

        _atkController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                Vector3 startPos = transform.position;
                Vector3 endPos = target.transform.position;
                Vector3 center = (startPos + target.transform.position) * 0.5f;
                center -= new Vector3(0, 10, 0);
                startPos = startPos - center;
                endPos = endPos - center;

                transform.position = Vector3.Slerp(startPos, endPos, 0.05f);
                //transform.position = Vector3.Slerp(transform.position, target.transform.position, 0.05f);
                transform.position += center;
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

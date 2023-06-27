using System;
using UnityEngine;
using UniRx;

public class Projectile_Stun : MonoBehaviour
{
    public float power;
    public int durationTime;  // ������ ���ӽð�
    public string idName;

    public Animator anim;

    public int slowDuration = 1;    // ���ο� ���ӽð�
    public float slowPercent = 0.2f;   // ���ο� %
    public float rotateSpeed = 2f;      // ȸ�� �ӵ�(n�ʿ� 1����)

    private IDisposable _atkController = Disposable.Empty;
    private IDisposable _lookAtTimer = Disposable.Empty;
    private AI_Structure ai_Structure;
    public BoxCollider coll;

    // �����ϴµ� ���������� Ÿ�̸� ����
    private void StopAttack()
    {
        _atkController.Dispose();
        _atkController = Disposable.Empty;

        _lookAtTimer.Dispose();
        _atkController = Disposable.Empty;
    }

    public void ReadyAndShot(AI_Structure structure, Transform target)
    {
        int remainTime = durationTime;

        StopAttack();

        anim.SetTrigger("LaunchStart");
        ai_Structure = structure;

        LookAtTarget(target);
        _atkController = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                remainTime--;

                if (remainTime == 1)
                {
                    Debug.Log("�׸� ȣ��");
                    anim.SetTrigger("LaunchEnd");
                }
                else if (remainTime == 0)
                {
                    GamePlay.Instance.spawnManager.ReturnProjectilePool(idName, this.transform);
                    StopAttack();
                }
            });

        var vec = Vector3.zero;
        _lookAtTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                //LookAtTarget(target);
                vec.y = Time.deltaTime * 360 / rotateSpeed;
                transform.Rotate(vec);
            });
    }

    private void LookAtTarget(Transform target)
    {
        var targetPos = target.position + new Vector3(0, -1.5f, 0);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ai_Structure.DealCrowdControl(other.gameObject, GameDefine.CCType.Slow, slowDuration, slowPercent);
            ai_Structure.DealDamage(other.gameObject);
            Debug.Log("��ȭ ����");
        }
    }
}

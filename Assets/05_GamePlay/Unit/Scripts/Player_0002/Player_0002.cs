using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;

public class Player_0002 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;
    private IDisposable _moveTimer = Disposable.Empty;

    public NavMeshAgent nav;
    public float detectRange;   // ���� ���� ����
    public Animator anim;
    public NormalAttack_0002 waterShot;
    public SkillAttack_0002 skill1002;
    private Vector3 targetPos;
    private GameObject _target = null;

    private bool _isSkill = false;

    private void Start()
    {
        DetectEnemyStart();
    }

    private void StopPlayer()
    {
        _atkTimer.Dispose();
        _atkTimer = Disposable.Empty;
    }

    private void StopMove()
    {
        _moveTimer.Dispose();
        _moveTimer = Disposable.Empty;
    }

    public void DetectEnemyStart()
    {
        Debug.Log("DetectEnemyStart");
        _isSkill = false;
        StopPlayer();     // Ÿ�̸� �Ͻ� ����
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                ChaseEnemy();
            });
    }

    public void DetectEnemyStart_Skill()
    {
        Debug.Log("DetectEnemyStart_Skill");
        _isSkill = true;
        StopPlayer();     // Ÿ�̸� �Ͻ� ����
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                ChaseEnemy();
            });
    }

    private void ChaseEnemy()
    {
        Debug.Log("ChaseEnemy");
        Collider[] hitCol = Physics.OverlapSphere(transform.position, detectRange);

        Debug.Log("HitCol : " + hitCol.Length);

        bool isCheck = false;   // ������ ����� �ִ���?

        for (int i = 0; i < hitCol.Length; i++)
        {
            Debug.Log("ColName : " + hitCol[i].name);
            if (hitCol[i].gameObject.CompareTag("Monster") == true)
            {
                isCheck = true;
                _target = hitCol[i].gameObject;
                break;
            }
        }

        if (isCheck == true)
        {
            Debug.Log(_target.name);
            StopPlayer();
            //stateType = GameDefine.Unit0001StateType.MoveToRepair;
            targetPos = _target.transform.position;
            var vec = transform.position - targetPos;

            if(Vector3.Distance(transform.position, targetPos) > 13.5f)
            {
                var pos = targetPos + (vec.normalized * 13.5f);    // 13.5f �� �����Ÿ� ���̼�ġ. �̰� �����ؾ� ��
                nav.SetDestination(pos);
                anim.SetInteger("animation", 18);    // �޸��� �ִϸ��̼�
            }
            else
            {
                transform.LookAt(targetPos);
            }
            Invoke("Skill_WaterShot", 1.2f);
        }
    }
    public void Skill_WaterShot()
    {
        Debug.Log("Skill_WaterShot");
        StopMove();
        if (_target.gameObject.activeSelf == true)
        {
            anim.SetInteger("animation", 51);    // �޸��� �ִϸ��̼�
            transform.LookAt(targetPos);

            if(_isSkill == true)
            {
                waterShot.AttackStart();
            }
            else
            {
                skill1002.SkillAttackStart();
            }

            Invoke("Skill_WaterShot", 1.2f);
        }
        else
        {
            DetectEnemyStart();
        }
    }
}

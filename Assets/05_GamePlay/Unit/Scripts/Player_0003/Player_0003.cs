using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;

public class Player_0003 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;
    private IDisposable _moveTimer = Disposable.Empty;

    public NavMeshAgent nav;
    public float detectRange;   // 공격 감지 범위
    public Animator anim;   
    private NormalAttack_0003 normal1003;
    private SkillAttack_0003 skill1003;
    private Vector3 targetPos;
    private GameObject _target = null;

    private bool _isSkill = false;

    private void Start()
    {
        normal1003 = GamePlay.Instance.skillManager.normalAtk_1003;
        skill1003 = GamePlay.Instance.skillManager.skillAtk_1003;
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

    public void SetSkill()
    {
        _isSkill = true;
    }

    public void DetectEnemyStart()
    {
        Debug.Log("DetectEnemyStart");
        _isSkill = false;
        anim.SetInteger("animation", 1);    // Idle 애니메이션
        StopPlayer();     // 타이머 일시 종료
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject)
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
        anim.SetInteger("animation", 1);    // Idle 애니메이션
        StopPlayer();     // 타이머 일시 종료
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(0.5f)).TakeUntilDisable(gameObject)
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

        bool isCheck = false;   // 공격할 대상이 있는지?

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

            if (Vector3.Distance(transform.position, targetPos) > 13.5f)
            {
                var pos = targetPos + (vec.normalized * 13.5f);    // 13.5f 는 사정거리 더미수치. 이거 수정해야 함
                nav.SetDestination(pos);
                anim.SetInteger("animation", 18);    // 달리기 애니메이션
            }
            else
            {
                transform.LookAt(targetPos);
            }
            Invoke("UnitAtk_0003", 1.2f);
        }
    }

    private AIPlayer monster = null;
    public void UnitAtk_0003()
    {
        Debug.Log("UnitAtk_0003");

        if(monster == null)
        {
            monster = _target.GetComponent<AIPlayer>();
        }

        StopMove();
        Debug.Log(monster.GetStateType());
        if (monster.GetStateType() != GameDefine.AIStateType.Die)
        {
            anim.SetInteger("animation", 50);    // 점프 애니메이션
            transform.LookAt(targetPos);

            if (_isSkill == false)
            {
                normal1003.transform.position = targetPos + new Vector3(0, 5, 0);
                normal1003.SpawnCloud();
                DealCrowdControl(_target, GameDefine.CCType.Slow, 1.5f, 0.2f);
                DealDamage(_target);

                Invoke("UnitAtk_0003", 1.2f);
            }
            else
            {
                skill1003.transform.position = targetPos + new Vector3(0, -5, 0);
                skill1003.SkillAttackStart();
                _isSkill = false;
                Invoke("UnitAtk_0003", 2.7f);
            }
        }
        else
        {
            monster = null;
            DetectEnemyStart();
        }
    }
}

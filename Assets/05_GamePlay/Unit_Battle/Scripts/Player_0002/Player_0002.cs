using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;

public class Player_0002 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;
    private IDisposable _moveTimer = Disposable.Empty;

    public NavMeshAgent nav;
    public float detectRange;   // 공격 감지 범위
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

    public void SetSkill()
    {
        _isSkill = true;
    }

    public void DetectEnemyStart()
    {
        Debug.Log("DetectEnemyStart");
        _isSkill = false;
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
        StopPlayer();     // 타이머 일시 종료
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

            if(Vector3.Distance(transform.position, targetPos) > 13.5f)
            {
                var pos = targetPos + (vec.normalized * 13.5f);    // 13.5f 는 사정거리 더미수치. 이거 수정해야 함
                nav.SetDestination(pos);
                anim.SetInteger("animation", 18);    // 달리기 애니메이션
            }
            else
            {
                transform.LookAt(targetPos);
            }
            Invoke("UnitAtk_0002", 1.2f);
        }
    }

    private AIPlayer monster = null;
    public void UnitAtk_0002()
    {
        Debug.Log("UnitAtk_0002");

        if (monster == null)
        {
            monster = _target.GetComponent<AIPlayer>();
        }

        StopMove();

        if (CheckMonsterDie(monster) == false)
        {
            anim.SetInteger("animation", 51);    // 조준 애니메이션
            transform.LookAt(targetPos);

            if(_isSkill == false)
            {
                waterShot.AttackStart();

                if (CheckMonsterDie(monster) == false)
                    Invoke("UnitAtk_0002", 1.2f);
                else
                {
                    monster = null;
                    DetectEnemyStart();
                }

                Invoke("UnitAtk_0002", 1.2f);
            }
            else
            {
                skill1002.SkillAttackStart();

                _isSkill = false;

                if (CheckMonsterDie(monster) == false)
                    Invoke("UnitAtk_0002", 2.7f);
                else
                {
                    monster = null;
                    DetectEnemyStart();
                }
            }
        }
        else
        {
            DetectEnemyStart();
        }
    }

    private bool CheckMonsterDie(AIPlayer monster)
    {
        return monster.GetStateType() == GameDefine.AIStateType.Die;
    }
}

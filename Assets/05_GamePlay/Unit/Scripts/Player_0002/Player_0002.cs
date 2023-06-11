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
    public GameObject waterShot;
    private Vector3 targetPos;
    private GameObject _target = null;

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
        StopPlayer();     // 타이머 일시 종료
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                ChaseEnemy();
            });
    }

    private void MoveTimerStart()
    {
        StopMove();
        transform.LookAt(targetPos);
        Debug.Log("MoveTimerStart");
        _moveTimer = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.5f);
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
            Invoke("Skill_WaterShot", 1.2f);
        }
    }
    public void Skill_WaterShot()
    {
        Debug.Log("Skill_WaterShot");
        StopMove();
        if (_target.gameObject.activeSelf == true)
        {
            anim.SetInteger("animation", 51);    // 달리기 애니메이션
            //transform.LookAt(targetPos);
            waterShot.SetActive(true);
        }
        else
        {
            DetectEnemyStart();
        }
    }

}

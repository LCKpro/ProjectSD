using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;


public class Player_0001 : Stat
{
    private GameDefine.Unit0001StateType stateType = GameDefine.Unit0001StateType.None;

    private IDisposable _playerTimer = Disposable.Empty;
    private IDisposable _moveTimer = Disposable.Empty;

    private AI_Structure _target;

    public NavMeshAgent nav;
    public float detectRange;   // 건물 수리 감지 범위
    public Animator anim;
    private Vector3 targetPos;
    private float repairTime = 2f;

    private void Start()
    {
        DetectRepairStart();
    }

    private void StopPlayer()
    {
        _playerTimer.Dispose();
        _playerTimer = Disposable.Empty;
    }

    private void StopMove()
    {
        _moveTimer.Dispose();
        _moveTimer = Disposable.Empty;
    }

    public void DetectRepairStart()
    {
        Debug.Log("DetectRepairStart");
        StopPlayer();     // 타이머 일시 종료
        _playerTimer = Observable.Interval(TimeSpan.FromSeconds(2f)).TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                if(stateType == GameDefine.Unit0001StateType.None)
                {
                    CheckRepair();
                }
            });
    }

    private void CheckRepair()
    {
        //Debug.Log("CheckRepair");
        Collider[] hitCol = Physics.OverlapSphere(transform.position, detectRange);

        //Debug.Log("HitCol : " + hitCol.Length);

        float minHPRaet = 1f;
        AI_Structure structure = null;
        bool isCheck = false;   // 고칠게 있는지?

        for (int i = 0; i < hitCol.Length; i++)
        {
            //Debug.Log("ColName : " + hitCol[i].name);
            if (hitCol[i].gameObject.CompareTag("Breakable") == true)
            {
                var aiStructure = hitCol[i].GetComponentInParent<AI_Structure>();
                var rate = aiStructure.GetStructureHpRate();
                //Debug.Log("Rate : " + rate);

                if (minHPRaet > rate)
                {
                    isCheck = true;
                    minHPRaet = rate;
                    structure = aiStructure;
                }
            }
        }

        if(isCheck == true)
        {
            //Debug.Log("CheckRepair Check OK");
            stateType = GameDefine.Unit0001StateType.MoveToRepair;
            targetPos = structure.transform.position;
            var vec = transform.position - targetPos;
            _target = structure;

            repairTime = 2f;
            targetPos = targetPos + (vec.normalized * 3.5f);
            anim.SetInteger("animation", 18);    // 달리기 애니메이션
            nav.SetDestination(targetPos);
            MoveTimerStart();
            Invoke("Skill_Repair", 1.2f);
        }
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
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.1f);
            });
    }

    public void Skill_Repair()
    {
        //Debug.Log("Skill_Repair");
        StopMove();
        anim.SetInteger("animation", 28);    // 달리기 애니메이션
        _target.RepairStructure(repairTime, this);
    }

    /// <summary>
    /// 수리 완료 후 다시 Idle 상태로 전환
    /// </summary>
    public void ChangeStateToIdle()
    {
        //Debug.Log("ChangeStateToIdle");
        anim.SetInteger("animation", 1);      // Idle 1
        stateType = GameDefine.Unit0001StateType.None;
    }

    public void ActiveSkill0001(AI_Structure obj)
    {
        StopMove();
        StopPlayer();
        stateType = GameDefine.Unit0001StateType.MoveToRepair;
        targetPos = obj.transform.position;
        var vec = transform.position - targetPos;
        _target = obj;

        repairTime = 10f;
        targetPos = targetPos + (vec.normalized * 3.5f);
        anim.SetInteger("animation", 18);    // 달리기 애니메이션
        nav.SetDestination(targetPos);
        MoveTimerStart();
        Invoke("Skill_Repair", 1.2f);
    }
}

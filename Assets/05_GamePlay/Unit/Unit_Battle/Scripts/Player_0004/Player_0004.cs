using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class Player_0004 : Stat
{
    private IDisposable _atkTimer = Disposable.Empty;
    private IDisposable _moveTimer = Disposable.Empty;

    public NavMeshAgent nav;
    public float detectRange;   // 공격 감지 범위
    public Animator anim;
    public GameObject redLight;
    public int skillHitLimit = 5;
    public Transform nozzlePos;
    private Vector3 targetPos;
    private GameObject _target = null;

    private bool _isSkill = false;

    private void Start()
    {
        DetectEnemyStart();
    }

    private void StopPlayer()
    {
        anim.SetInteger("animation", 32);    // Idle 애니메이션
        _atkTimer.Dispose();
        _atkTimer = Disposable.Empty;
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
    }

    public void SetSkill()
    {
        _isSkill = true;
    }

    private float detectTime = 1.5f;
    public void DetectEnemyStart()
    {
        Debug.Log("DetectEnemyStart");
        _isSkill = false;
        anim.SetInteger("animation", 32);    // Idle 애니메이션
        StopPlayer();     // 타이머 일시 종료
        _atkTimer = Observable.Interval(TimeSpan.FromSeconds(detectTime)).TakeUntilDisable(gameObject)
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
                detectTime = 0.5f;  // 적 있으면 다음 감지는 더 짧게
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
                anim.SetInteger("animation", 21);    // 걷기 애니메이션
            }
            else
            {
                transform.LookAt(targetPos);
            }
            Invoke("UnitAtk_0004", 1.2f);
        }
        else
        {
            detectTime = 1.5f;  // 적 감지가 안되었으면 감지를 덜 하도록
        }
    }

    private AIPlayer monster = null;
    public void UnitAtk_0004()
    {
        Debug.Log("UnitAtk_0004");

        if (monster == null)
        {
            monster = _target.GetComponent<AIPlayer>();
        }

        StopPlayer();
        if (CheckMonsterDie(monster) == false)
        {
            if(_isSkill == false)
            {
                NormalAtk();
            }
            else
            {
                SkillAtk();
            }
        }
        else
        {
            monster = null;
            ChaseEnemy();
        }
    }

    private void NormalAtk()
    {
        anim.SetInteger("animation", 53);    // 총 쏘는 애니메이션
        transform.LookAt(targetPos);
        _isSkill = false;

        if (CheckMonsterDie(monster) == false)
            Invoke("UnitAtk_0004", 1.2f);
        else
        {
            monster = null;
            ChaseEnemy();
        }

    }

    private GameObject[] _hitTarget = new GameObject[5] { null, null, null, null, null };
    private void SkillAtk()
    {
        Collider[] hitCol = Physics.OverlapSphere(transform.position, detectRange);

        int hitCount = 0;   // 최대 공격 가능한 대상

        for (int i = 0; i < hitCol.Length; i++)
        {
            if (hitCol[i].gameObject.CompareTag("Monster") == true)
            {
                _hitTarget[hitCount] = hitCol[i].gameObject;
                hitCount++;
                hitCol[i].GetComponent<AIPlayer>().SetDieMark();

                Debug.Log("0004 스킬" + hitCount + "명 찾음");

                if (hitCount >= skillHitLimit)   // 5명 다 차면 스탑
                {
                    break;
                }
            }
        }

        redLight.SetActive(true);
        anim.SetInteger("animation", 52);    // 총 뽑는 애니메이션
        transform.LookAt(targetPos);

        if (CheckMonsterDie(monster) == false)
            Invoke("UnitAtk_0004", 2.7f);
        else
        {
            monster = null;
            ChaseEnemy();
        }
    }

    public void OnEvent_NormalAtk()
    {
        if(_isSkill == true)
        {
            return;
        }

        _isSkill = false;

        var projectile = GamePlay.Instance.poolManager_Projectile.GetFromPool<Transform>("P0004");

        projectile.position = nozzlePos.position;
        projectile.LookAt(monster.transform.position);
        projectile.GetComponent<Rigidbody>().AddForce(monster.transform.position * 2f, ForceMode.Impulse);
    }

    public void OnEvent_SkillAtk()
    {
        if (_isSkill == false)
        {
            return;
        }

        _isSkill = false;

        damageValue = 200;

        for (int i = 0; i < _hitTarget.Length; i++)
        {
            if (_hitTarget[i] == null)
            {
                break;
            }

            DealDamage(_hitTarget[i].gameObject);
        }

        damageValue = 75;
        redLight.SetActive(false);
    }

    /// <summary>
    /// true = 죽음  false = 안죽음
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private bool CheckMonsterDie(AIPlayer monster)
    {
        return monster.GetStateType() == GameDefine.AIStateType.Die;
    }
}

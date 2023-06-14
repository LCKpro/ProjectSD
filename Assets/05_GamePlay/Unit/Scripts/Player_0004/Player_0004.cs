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
    public float detectRange;   // ���� ���� ����
    public Animator anim;
    public GameObject redLight;
    public int skillHitLimit = 5;
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

    private float detectTime = 1.5f;
    public void DetectEnemyStart()
    {
        Debug.Log("DetectEnemyStart");
        _isSkill = false;
        anim.SetInteger("animation", 1);    // Idle �ִϸ��̼�
        StopPlayer();     // Ÿ�̸� �Ͻ� ����
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

        bool isCheck = false;   // ������ ����� �ִ���?

        for (int i = 0; i < hitCol.Length; i++)
        {
            Debug.Log("ColName : " + hitCol[i].name);
            if (hitCol[i].gameObject.CompareTag("Monster") == true)
            {
                isCheck = true;
                _target = hitCol[i].gameObject;
                detectTime = 0.5f;  // �� ������ ���� ������ �� ª��
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
                var pos = targetPos + (vec.normalized * 13.5f);    // 13.5f �� �����Ÿ� ���̼�ġ. �̰� �����ؾ� ��
                nav.SetDestination(pos);
                anim.SetInteger("animation", 21);    // �ȱ� �ִϸ��̼�
            }
            else
            {
                transform.LookAt(targetPos);
            }
            Invoke("UnitAtk_0004", 1.2f);
        }
        else
        {
            detectTime = 1.5f;  // �� ������ �ȵǾ����� ������ �� �ϵ���
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

        StopMove();
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
        anim.SetInteger("animation", 52);    // �� �̴� �ִϸ��̼�
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

    private void SkillAtk()
    {
        Collider[] hitCol = Physics.OverlapSphere(transform.position, detectRange);

        int hitCount = 0;   // �ִ� ���� ������ ���
        GameObject[] hitTarget = new GameObject[5] { null, null, null, null, null };

        for (int i = 0; i < hitCol.Length; i++)
        {
            Debug.Log("ColName : " + hitCol[i].name);
            if (hitCol[i].gameObject.CompareTag("Monster") == true)
            {
                hitTarget[hitCount] = hitCol[i].gameObject;
                hitCount++;

                if(hitCount >= skillHitLimit)   // 5�� �� ���� ��ž
                {
                    break;
                }
            }
        }

        redLight.SetActive(true);
        anim.SetInteger("animation", 52);    // �� �̴� �ִϸ��̼�
        transform.LookAt(targetPos);

        for (int i = 0; i < hitTarget.Length; i++)
        {
            if(hitTarget[i] == null)
            {
                break;
            }
        }


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


    }

    public void OnEvent_SkillAtk()
    {
        if (_isSkill == false)
        {
            return;
        }

        _isSkill = false;
    }

    /// <summary>
    /// true = ����  false = ������
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private bool CheckMonsterDie(AIPlayer monster)
    {
        return monster.GetStateType() == GameDefine.AIStateType.Die;
    }
}

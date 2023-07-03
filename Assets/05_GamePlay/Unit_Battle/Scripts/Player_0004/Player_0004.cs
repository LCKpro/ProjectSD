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
        anim.SetInteger("animation", 32);    // Idle �ִϸ��̼�
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
        anim.SetInteger("animation", 32);    // Idle �ִϸ��̼�
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
        anim.SetInteger("animation", 53);    // �� ��� �ִϸ��̼�
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

        int hitCount = 0;   // �ִ� ���� ������ ���

        for (int i = 0; i < hitCol.Length; i++)
        {
            if (hitCol[i].gameObject.CompareTag("Monster") == true)
            {
                _hitTarget[hitCount] = hitCol[i].gameObject;
                hitCount++;
                hitCol[i].GetComponent<AIPlayer>().SetDieMark();

                Debug.Log("0004 ��ų" + hitCount + "�� ã��");

                if (hitCount >= skillHitLimit)   // 5�� �� ���� ��ž
                {
                    break;
                }
            }
        }

        redLight.SetActive(true);
        anim.SetInteger("animation", 52);    // �� �̴� �ִϸ��̼�
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
    /// true = ����  false = ������
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private bool CheckMonsterDie(AIPlayer monster)
    {
        return monster.GetStateType() == GameDefine.AIStateType.Die;
    }
}

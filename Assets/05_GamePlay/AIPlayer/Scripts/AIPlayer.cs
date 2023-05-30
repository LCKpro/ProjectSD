using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Redcode.Pools;
using UniRx;
using Random = UnityEngine.Random;


public class AIPlayer : MonoBehaviour, IPoolObject
{
    public string idName;
    public Animator anim;
    public Transform target;
    private bool _isAtDestination;
    private Rigidbody _rigid = null;

    private NavMeshAgent _ai;

    private IDisposable _moveController = Disposable.Empty;

    private void Start()
    {
        _ai = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
    }

    private void FinalizeControl()
    {
        _moveController.Dispose();
        _moveController = Disposable.Empty;
    }

    private void Init()
    {
        //TODO::등장하는 위치 정하고 설정해야 함
        //transform.position = Vector3.zero;
        //Transform[] spawnPos = GameManager.instance.points;
        //ai.SetDestination(spawnPos[Random.Range(0, spawnPos.Length)].position);
        target = GamePlay.Instance.playerManager.GetPlayer().transform;
        SetPosition();
        AIControllerStart();
    }


    private void SetPosition()
    {
        int x, z;

        var r = GameUtils.RandomBool();
        if(r)
        {
            x = Random.Range(0, 23);
            z = 30;

            if (x % 2 == 0) // x좌표는 좌우 구분을 위해 음수도 추가되어야 함
            {
                x *= -1;
            }
        }
        else
        {
            x = 23;
            z = Random.Range(0, 30);

            if (z % 2 == 0) // x좌표는 좌우 구분을 위해 음수도 추가되어야 함
            {
                x *= -1;
            }
        }

        var pos = new Vector3(target.position.x + x, 0, target.position.z + z);
        Debug.Log("적 위치 : " + pos);
        transform.position = pos;
    }

    public void AIControllerStart()
    {
        FinalizeControl();
        anim.SetInteger("animation", 15);
        _moveController = Observable.EveryUpdate().TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                AIMove();
            });
    }

    private void AIMove()
    {
        _ai.SetDestination(target.position);
    }


    /// <summary>
    /// 스스로 사라지도록 수정
    /// </summary>
    private void FinalizeAI()
    {
        GamePlay.Instance.poolManagerA.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FinalizeControl();
            FinalizeAI();
            //transform.gameObject.SetActive(false);
        }

        // 부딪히면 부수는 행동으로 변경
    }

    public void OnCreatedInPool()
    {
    }

    public void OnGettingFromPool()
    {
        Init();
    }

    // State 만들어야 함~
}

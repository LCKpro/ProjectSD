using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;

public class Unit_Day : MonoBehaviour
{
    public enum UnitStateType
    {
        Idle = 0,
        LookAround = 1,
        Walk = 2,
        Cute = 3,
        Quadruped_Chilling = 4,
        Make = 5,
        None = 99,
    }

    public class UnitState
    {
        public UnitStateType stateType = UnitStateType.None;
        public int animNumber;
        public int faceIndex;

        public UnitState(UnitStateType type, int number, int index)
        {
            stateType = type;
            animNumber = number;
            faceIndex = index;
        }
    }

    private IDisposable _stateTimer = Disposable.Empty;
    private UnitState[] unitStateList = new UnitState[6]
    {
        new UnitState(UnitStateType.Idle, 0, 0),
        new UnitState(UnitStateType.LookAround, 1, 1),
        new UnitState(UnitStateType.Walk, 2, 0),
        new UnitState(UnitStateType.Cute, 3, 2),
        new UnitState(UnitStateType.Quadruped_Chilling, 4, 3),
        new UnitState(UnitStateType.Make, 5, 4)
    };

    public Animator catAnimation;
    public Rigidbody rigid;
    public Transform[] direct;

    public NavMeshAgent _nav;

    public Material[] faceList;
    public SkinnedMeshRenderer skinRenderer;

    private void Start()
    {
        StartState();
    }

    private void OnEnable()
    {
        StartState();
    }

    private void OnDisable()
    {
        StopState();
    }

    private void StopState()
    {
        _stateTimer.Dispose();
        _stateTimer = Disposable.Empty;
    }

    private void StartState()
    {
        StopState();
        RndState();
        _stateTimer = Observable.Interval(TimeSpan.FromSeconds(5f))
            .TakeUntilDisable(gameObject)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
            {
                RndState();
            });
    }

    private bool isWalk = false;
    private Vector3 prevDir = Vector3.zero;
    private void RndState()
    {
        rigid.velocity = Vector3.zero;

        if (isWalk == false)
        {
            _nav.isStopped = false;
            catAnimation.SetInteger("animation", 2);

            var rndDir = RandomVector();

            if(prevDir == rndDir)
            {
                rndDir *= (-1);
            }
            else
            {
                prevDir = rndDir;
            }

            _nav.SetDestination(transform.position + rndDir * 15);
            skinRenderer.materials[1].CopyPropertiesFromMaterial(faceList[0]);

            isWalk = true;

            Debug.Log("Walk Dir : " + rndDir);
        }
        else
        {
            _nav.isStopped = true;

            var rnd = GameUtils.RandomItem(unitStateList);

            catAnimation.SetInteger("animation", rnd.animNumber);
            skinRenderer.materials[1].CopyPropertiesFromMaterial(faceList[rnd.faceIndex]);
            isWalk = false;

            Debug.Log("Type : " + rnd.stateType + " Anim : " + rnd.animNumber);
        }
    }

    private Vector3 RandomVector()
    {
        System.Random random = new System.Random();

        var rnd = random.Next(4);

        switch (rnd)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.back;
            case 2:
                return Vector3.left;
            case 3:
                return Vector3.right;
            default:
                return Vector3.forward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isWalk = false;
        StartState();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MummyBufferStateItem;
using System;
using Photon.Pun;

public class MummyBufferController : BaseMonsterController
{
    [Header("RightHand")]
    [SerializeField] public GameObject StoneSpawned;

    #region STATE
    public State IDLE_STATE;
    public State IDLE_BATTLE_STATE;
    public State CHASE_STATE;
    public State ATTACK_STATE;
    public State COUNTER_ENABLE_STATE;
    public State BUFF_STATE;
    public State GROGGY_STATE;
    public State DIE_STATE;
    public State GLOBAL_STATE;
    #endregion

    #region STATE VARIABLE
    [SerializeField] private const float _threadHoldBuff = 12.0f;
    private float _buffTime = _threadHoldBuff;

    public float BuffTime { get => _buffTime; set => _buffTime = value; }
    public float ThreadHoldBuff { get => _threadHoldBuff; }
    #endregion

    private MummyBufferAnimationData _animData;
    public MummyBufferAnimationData AnimData { get => _animData; }
    public Action OnDeath;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Start()
    {
        base.Start();
        ChangeState(IDLE_STATE);
    }

    // ---------------------------------- Init ------------------------------------------
    protected override void Init()
    {
        _animData = GetComponent<MummyBufferAnimationData>();
        _animData.StringAnimToHash();

        // ----------------------------- Animation && State -------------------------------------

        _stateMachine = new StateMachine();

        IDLE_STATE = new IdleState(this);
        IDLE_BATTLE_STATE = new IdleBattleState(this);
        CHASE_STATE = new ChaseState(this);
        ATTACK_STATE = new AttackState(this);
        BUFF_STATE = new BuffState(this);
        DIE_STATE = new DieState(this);
        GLOBAL_STATE = new GlobalState(this);

        COUNTER_ENABLE_STATE = new CounterEnableState(this);
        GROGGY_STATE = new GroggyState(this);

        _stateMachine.SetGlobalState(GLOBAL_STATE);

        Detector.DetectRange *= 2;
        Agent.stoppingDistance = Detector.AttackRange;      // 공격 사거리와 멈추는 거리를 같게 세팅
        UnitType = Define.UnitType.MummyManBuffer;
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
        // 죽었을 경우 보내는 ACTION
        OnDeath?.Invoke();
        Debug.Log("Buffer Die");

        base.DestroyEvent();
    }


    public void ChangeToIdleState()
    {
        photonView.RPC("RPC_ChangeToIdleState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToIdleState()
    {
        ChangeState(IDLE_STATE);
    }
    // ---------
    public void ChangeToIdleBattleState()
    {
        photonView.RPC("RPC_ChangeToIdleBattleState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToIdleBattleState()
    {
        ChangeState(IDLE_BATTLE_STATE);
    }


    public void ChangeToChaseState()
    {
        photonView.RPC("RPC_ChangeToChaseState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToChaseState()
    {
        ChangeState(CHASE_STATE);
    }



    public void ChangeToAttackState()
    {
        photonView.RPC("RPC_ChangeToAttackState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToAttackState()
    {
        ChangeState(ATTACK_STATE);
    }


    public void ChangeToBuffState()
    {
        photonView.RPC("RPC_ChangeToBuffState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToBuffState()
    {
        ChangeState(BUFF_STATE);
    }

    public void ChangeToCounterEnableState()
    {
        photonView.RPC("RPC_ChangeToCounterEnableState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToCounterEnableState()
    {
        ChangeState(COUNTER_ENABLE_STATE);
    }

    public void ChangeToGroggyState()
    {
        photonView.RPC("RPC_ChangeToGroggyState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToGroggyState()
    {
        ChangeState(GROGGY_STATE);
    }


    public void ChangeToDieState()
    {
        photonView.RPC("RPC_ChangeToDieState", RpcTarget.Others);
    }
    [PunRPC]
    void RPC_ChangeToDieState()
    {
        ChangeState(DIE_STATE);
    }

    [PunRPC]
    void RPC_TakeDamage(int attackDamage, bool isCounter, int shield, bool evasion, int defense)
    {
        CalcDamage(attackDamage, isCounter, shield, evasion, defense);
    }



}

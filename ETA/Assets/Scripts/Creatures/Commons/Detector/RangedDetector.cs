using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// [ 한 명만 타겟팅 ]
/// 1. 현재 던전에 있는 플레이어를 배열로 저장한다.
/// 2. 0.1초마다 detectRange에 플레이어가 있는지 확인한다.
/// 3. 플레이어가 있는 경우, 1.5초 동안 가까운 적을 타겟팅한다.
/// 4.                     1.5초 이후에는 플레이어 배열 중 가장 먼 거리의 플레이어를 타겟팅한다.
/// </summary>
public class RangedDetector : MonoBehaviour, IDetector
{
    [Header("Set Values from the Inspector")]
    [SerializeField] private float _detectRange = 18.0f;
    [SerializeField] private float _attackRange = 14.0f;
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask TargetLayerMask;
    //[SerializeField] private PlayerController[] _players;

    private bool _hasMetTargetOne;          // 타겟과 첫 조우 여부
    private float _waitSeconds;             // 기다린 시간
    private bool _isWaitComplete;           // 1.5초 기다렸나

    public float DetectRange { get => _detectRange; set => _detectRange = value; }
    public float AttackRange { get => _attackRange; private set => _attackRange = value; }
    public Transform Target { get => _target; private set => _target = value; }

    private Ray _ray;

    private void Start()
    {
        _target = null;
        // 던전에 존재하는 플레이어를 저장한다. 게임 매니저를 이용해서 플레이어와 몬스터를 관리하고 싶음 -> 시간되면 겜매 구현
        //_players = FindObjectsOfType<PlayerController>();

        StartCoroutine(UpdateTarget());
    }

    private void Update()
    {
        if (!_isWaitComplete && _hasMetTargetOne )
        {
            WaitngDuration();
        }
    }

    private void WaitngDuration()
    {
        _waitSeconds += Time.deltaTime;
        if (_waitSeconds >= 1.5f)
        {
            _isWaitComplete = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!GetComponent<RangedDetector>().enabled) return;

        _ray.origin = transform.position;
        Gizmos.color = Color.red;
        if (Target == null ) Gizmos.DrawWireSphere(_ray.origin, _detectRange);
        else Gizmos.DrawWireSphere(_ray.origin, _attackRange);
    }

    // ---------------------------------- IDetector Functions -----------------------------------------------

    public IEnumerator UpdateTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GetComponent<PhotonView>().IsMine == false)
            {
                continue;
            }
            yield return null;

            Collider[] enemies = Physics.OverlapSphere(transform.position, _detectRange, TargetLayerMask);
            if (enemies.Length == 0)
            {
                _target = null;
            }
            
            if ( _hasMetTargetOne && _isWaitComplete )
            {
                // detectRange 안쪽과 attackRange 바깥쪽에 플레이어가 존재하도록 값을 세팅한다.
                // 그래야 모든 플레이어를 감지할 수 있기 때문이다.
                float farthestDist = 0;
                int viewId = -1;
                foreach (Collider player in enemies)
                {
                    // player.GetComponent<Collider>().enabled
                    if (player.GetComponent<Stat>().Hp > 0)
                    {
                        float distToEnemy = Vector3.Distance(transform.position, player.transform.position);
                        if (distToEnemy > farthestDist)
                        {
                            farthestDist = distToEnemy;
                            _target = player.transform;
                            viewId = _target.GetComponent<PhotonView>().ViewID;
                        }
                    }
                }
                if (viewId != -1) gameObject.GetComponent<PhotonView>().RPC("RPC_UpdateRangeTarget", RpcTarget.Others, viewId);


                //foreach (PlayerController player in _players)
                //{
                //    if (player.stat.Hp > 0)
                //    {
                //        float distToEnemy = Vector3.Distance(transform.position, player.transform.position);
                //        if (distToEnemy > farthestDist)
                //        {
                //            farthestDist = distToEnemy;
                //            _target = player.transform;
                //        }
                //    }
                //}
            }
            else
            {
                float closeDist = Mathf.Infinity;
                int viewId = -1;
                foreach (Collider enemy in enemies)
                {
                    _hasMetTargetOne = true;    // 한 번 Target을 만남

                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy < closeDist)
                    {
                        closeDist = distToEnemy;
                        _target = enemy.transform;
                        viewId = _target.GetComponent<PhotonView>().ViewID;
                    }
                }
                if (viewId != -1) gameObject.GetComponent<PhotonView>().RPC("RPC_UpdateRangeTarget", RpcTarget.Others, viewId);

            }
        }
    }

    public bool IsArriveToTarget()
    {
        if (_target == null) return false;
        return Vector3.Distance(_target.position, transform.position) <= _attackRange;
    }

    public bool IsArriveToTarget(Transform target, float attackRange)
    {
        _target = target;
        _attackRange = attackRange;
        if (_target == null) return false;
        return Vector3.Distance(_target.position, transform.position) <= _attackRange;
    }


    [PunRPC]
    void RPC_UpdateRangeTarget(int viewId)
    {

        PhotonView[] views = GameObject.FindObjectsOfType<PhotonView>();

        foreach (var view in views)
        {
            if (view.ViewID == viewId)
            {
                Target = view.transform;
                return;
            }
        }
    }
}

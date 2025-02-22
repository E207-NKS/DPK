using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// [ 한 명만 타겟팅 ]
/// 1. 0.1초마다 인식 범위에 적이 있는지 판단한다.
/// 2. 있으면 가장 가까운 적을 target으로 세팅한다.
/// </summary>
public class MeleeDetector : MonoBehaviourPunCallbacks, IDetector
{
    [Header("Set Values from the Inspector")]
    [SerializeField] private float _detectRange = 15.0f;
    [SerializeField] private float _attackRange;              // 근거리, 원거리, 일반과 보스 몬스터는 공격 사거리가 다르다.
    [SerializeField] private Transform _target;
    [SerializeField] public LayerMask TargetLayerMask;

    public float DetectRange { get => _detectRange; set => _detectRange = value; }
    public float AttackRange { get => _attackRange; private set => _attackRange = value; }
    public Transform Target { get => _target; private set => _target = value; }

    private Ray _ray;

    private void Start()
    {
        _target = null;

        StartCoroutine(UpdateTarget());

    }

    private void OnDrawGizmos()
    {
        if (!GetComponent<MeleeDetector>().enabled) return;

        _ray.origin = transform.position;
        Gizmos.color = Color.red;
        if (Target == null ) Gizmos.DrawWireSphere(_ray.origin, DetectRange);
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

            float closeDist = Mathf.Infinity;
            Collider[] enemies = Physics.OverlapSphere(transform.position, DetectRange, TargetLayerMask);
            if (enemies.Length  == 0)
            {
                _target = null;
            }
            else
            {
                int viewId = -1;
                foreach (Collider enemy in enemies)
                {
                    float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distToEnemy < closeDist)
                    {
                        closeDist = distToEnemy;
                        Target = enemy.transform;
                        viewId = Target.GetComponent<PhotonView>().ViewID;

                    }
                }
                if (viewId != -1) gameObject.GetComponent<PhotonView>().RPC("RPC_UpdateTarget", RpcTarget.Others, viewId);
            }
        }
    }

    public bool IsArriveToTarget()
    {
        if (_target == null) return false;
        float radius = _target.GetComponent<NavMeshAgent>().radius;
        Debug.Log(radius);
        Debug.Log(Vector3.Distance(_target.position, transform.position));
        return Vector3.Distance(_target.position, transform.position) <= _attackRange + radius;
    }

    public bool IsArriveToTarget(Transform target, float attackRange)
    {
        _target = target;
        _attackRange = attackRange;
        if (_target == null) return false;
        float radius = _target.GetComponent<NavMeshAgent>().radius;
        return Vector3.Distance(_target.position, transform.position) <= _attackRange + radius;
    }

    [PunRPC]
    void RPC_UpdateTarget(int viewId)
    {

        PhotonView[] views = GameObject.FindObjectsOfType<PhotonView>();

        foreach(var view in views)
        {
            if(view.ViewID == viewId)
            {
                Target = view.transform;
                return;
            }
        }
    }

}

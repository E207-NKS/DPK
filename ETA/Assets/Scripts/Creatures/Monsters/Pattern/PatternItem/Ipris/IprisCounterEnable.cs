using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IprisCounterEnable : Pattern
{
    IprisAnimationData _animData;
    IprisController _icontroller;

    [Header("개발 편의성")]
    [SerializeField] Vector3 _hitboxRange = new Vector3(2.0f, 4.0f, 2.0f);
    [SerializeField] float _upLoc = 2.0f;

    private float _duration;
    private int _penetration = 1;

    public override void Init()
    {
        base.Init();
        _animData = _controller.GetComponent<IprisAnimationData>();
        _icontroller = _controller.GetComponent<IprisController>();
        _duration = _animData.CounterEnableAnim.length * 2.0f;

        _createTime = 0.1f;
        _patternRange = _hitboxRange;
    }

    public override IEnumerator StartPatternCast()
    {
        Vector3 rootUp = transform.TransformDirection(Vector3.up * _upLoc);
        Vector3 objectLoc = transform.position + rootUp;

        yield return new WaitForSeconds(_createTime);

        _hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        _hitbox.SetUp(transform, _attackDamage, _penetration, false, _duration);
        _hitbox.transform.localScale = _patternRange;
        _hitbox.transform.rotation = transform.rotation;
        _hitbox.transform.position = objectLoc;


        _ps = Managers.Effect.Play(Define.Effect.CounterEnable, 0, _controller.transform);
        _ps.transform.position = _hitbox.transform.position;
        ParticleSystem.MainModule _psMainModule = _ps.main;
        _psMainModule.startLifetime = _animData.CounterEnableAnim.length * 2.0f;

        // 카운터 도중에 내는 소리
        Managers.Sound.Play("Monster/CounterEnergy_SND", Define.Sound.Effect);

        // 시전 도중에 카운터 스킬을 맞으면 hit box와 effect가 사라지고, sound가 발생
        float timer = 0;
        while (timer <= _duration)
        {
            if (_icontroller.IsHitCounter)
            {
                Managers.Resource.Destroy(_hitbox.gameObject);
                Managers.Effect.Stop(_ps);
                Managers.Effect.Play(Define.Effect.CounteredEffect_Blue, 0);
                Managers.Sound.Play("Monster/CounterEnable_SND", Define.Sound.Effect);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        Managers.Resource.Destroy(_hitbox.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private Coroutine blessingCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(20);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Blessing.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("BUFF2", 0.1f);
        Managers.Sound.Play("Skill/Heal");
        Managers.Coroutine.Run(BlessingCoroutine());

        yield return new WaitForSeconds(0.5f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    private IEnumerator BlessingCoroutine()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.BlessingEffect, 10.0f, gameObject.transform);
        if (ps != null)
            ps.transform.SetParent(gameObject.transform);

        _controller.IncreaseDefense(5 + Damage / 20);

        // 방어력 증가 후 일정 시간 대기
        yield return new WaitForSeconds(10.0f + (Damage / 100));
        _controller.DecreaseDefense(5 + Damage / 20);

        // 이전 코드
        /*
        BuffBox buffbox1 = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox1.SetUp(transform, 10, BuffBox.stat.Defense);
        buffbox1.transform.position = gameObject.transform.position;
        buffbox1.transform.localScale = new Vector3(1, 3, 1);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(buffbox1.gameObject);

        // 방어력 증가 후 일정 시간 대기
        yield return new WaitForSeconds(5.0f);

        // 방어력 감소 및 파티클 중지
        BuffBox buffbox2 = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox2.SetUp(transform, -10, BuffBox.stat.Defense);
        buffbox2.transform.position = gameObject.transform.position;
        buffbox2.transform.localScale = new Vector3(1, 3, 1);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(buffbox2.gameObject);
        Debug.Log($"Defense: {_controller.Stat.Defense}");
        */
    }
}

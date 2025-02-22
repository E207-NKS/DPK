using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNormalAttackSkill : Skill
{
    protected override void Init()
    {
        Damage = 20;
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(0.25f, 1.0f, 3f);

        base.Init();

    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        if (_controller.StateMachine.CurState is PlayerStates.SkillState) yield break;
        _animator.CrossFade("NORMAL_ATTACK", 0.05f);
        
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/NormalAttack");

        Managers.Effect.Play(Define.Effect.WarriorNormalAttackEffect, 0, gameObject.transform);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox.transform.rotation = gameObject.transform.rotation;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);

        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(1.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }
}

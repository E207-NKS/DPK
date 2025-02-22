using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : Skill
{



    protected override void Init()
    {
        SetCoolDownTime(10);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(7,7,7);
        // RangeType = Define.RangeType.Square;
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);

        
        yield return new WaitForSeconds(0.7f);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/WarriorStoneSpike");
        ParticleSystem ps = Managers.Resource.Instantiate("Effect/SpikeWaveStone").GetComponent<ParticleSystem>();
        ps.transform.position = hitbox.transform.position;
        ps.Play();

        yield return new WaitForSeconds(0.8f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}

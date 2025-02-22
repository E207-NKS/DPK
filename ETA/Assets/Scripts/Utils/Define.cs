using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SkillType
    {
        None,
        Target,
        Range,
        Holding,
        Immediately
    }

    public enum RangeType
    {
        Round,
        Square
    }
    public enum SkillKey
    {
        Q,
        W,
        E,
        R,
        A,
        S,
        D,
        F
    }

    public enum Sound
    {
        BGM,
        Effect,
        MaxCount
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Opening,
        Tutorial,
        StarShardPlain,
        ForgottenTemple,
        SeaOfAbyss,
        MultiPlayTest,
        Test
    }
    public enum UIEvent
    {
        Click,
        Drag,
        BeginDrag,
        EndDrag,
        Drop,
    }
    public enum MouseEvent
    {
        Press,
        Click
    }
    public enum CameraMode
    {
        QuarterVeiw
    }

    public enum  UnitType
    {
        // ------------------- 플레이어 -----------------------
        Player = 0,
        // ------------------- 소환수 ----------------------- 
        ForestSpirit,
        WindSpirit,
        // ------------------- 일반 몬스터 -----------------------
        Boar,
        Tree,
        Porin,
        Shaga,
        Butterfly,
        Starfish,
        Krake,
        Puffe,
        Octopus,
        Planta,
        Shellfish,
        // ------------------- 보스 몬스터 -----------------------
        FlowerDryad,
        Whale,
        KnightG,
        MummyMan,
        MummyManWarrior,
        MummyManBuffer,
        Ipris,
        Dragon,
    }

    public enum Effect
    {
        WarriorNormalAttackEffect,
        ArcherNormalAttackEffect,
        MageNormalAttackEffect,
        // ------------------------ KnightG ------------------------------
        KnightG_CounterAttack,
        KnightG_TwoSkillEnergy,
        KnightG_TwoSkillAttack,
        KnightG_PhaseTransition,
        KnightG_PhaseAttack,
        // ------------------------ Common ------------------------------
        CounterEnable,
        CounterEnable_Red,
        CounterEnable_Green,
        CounteredEffect_Blue,
        CounteredEffect_Red,
        CounteredEffect_Green,
        Groggy,
        // ------------------------ Common  ------------------------------
        EnergyNovaBlue,
        EnergyNovaGreen,
        AuraChargeYellow,
        AuraChargeBlue,
        AuraChargeGreen,
        AuraChargeRed,
        FlashLight,
        PoisonMist,
        Fear,
        WaterSlap,
        WaterSlapEffect,
        WaterSlapEffect1,
        BubbleShot,
        BubbleShotExplosion,
        QuickFreeze,
        Thunder1,
        Thunder2,
        Thunder3,
        ChainLightning,
        SpikeIce,
        Teleport,
        Plague,
        PlagueEffect,
        FireTrail,
        FireWall,
        BloodExplosion,
        Crater,
        Explosion,
        GroundCrash,
        BlessingEffect,
        HealEffect,
        SwordWaveWhite, 
        SlashWideBlue,
        SwordVolleyBlue,
        StoneSlash1,
        StoneSlash2,
        SurfaceExplosionDirtStone,
        WhirlwindEffect1,
        WhirlwindEffect2,
        Porin_Attack,
        WindShield,
        ArrowStab,
        RapidArrowShot,
        MagicSphereBlue,
        DarknessSlash,
        MagicMissileGreen,
        MagicMissileBlue,
        WaterSplashSoft,
        WaterSplatWide, 
        DoubleSlash1,
        DoubleSlash2,
        TripleSlash1,
        TripleSlash2,
        TripleSlash3,
        ThrowingShield,
        ShieldSlam,
        HolyHammerEffect2,
        MoonSwordEffect,
        QuadrupleSlash1,
        QuadrupleSlash2,
        QuadrupleSlash3,
        QuadrupleSlash4,
        LightningStrikeBlue,
        // ------------------------ Mummy ------------------------------
        MummyWarrior_WindMill,
        Mummy_RangedAttack,
        Mummy_RangedHit,
        Mummy_WindMill,
        Mummy_JumpAura,
        Mummy_JumpDown,
        Mummy_Shouting,
        Mummy_Clap,
        Mummy_Rushing,
        Mummy_RushEnd,
        Mummy_Buff,
        Mummy_Shield,
        // ------------------------ Mummy ------------------------------
        WindBall,
        FlowerDryadAttack,
        FlowerDryad_CounterAttack,
        LightningShot,
        ArrowShower,
        RapidArrowHit,
        RapidArrowCharge,
        ArrowBomb,
        CollavoWindBall,
        ForestSpiritSpawn,
        WindSlash,
        CollavoCyclone,
        CollavoCycloneShot,
        CollavoBlackHole,
        Gravity01,
        Gravity02,
        SparkleMissileGreen,
        ArcherMageUIEffect,
        WarriorMageUIEffect,
        WarriorArcherUIEffect,
        ScatterArrow,
        WindBlast,
        ChargeArrow,
        ChargeArrowCharge,
        ForestBless,
        ForestBlessAura,
        Tempest,
        WindSpiritAttack,
        StormStrikeCharge,
        StormStrikeFire01,
        StormStrikeFire02,
        StormStrikeHit,
        // ------------------------ Ipris ------------------------------
        Ipris_Buff,
        Ipris_Shield,
        Ipris_CounterAttack,
        Ipris_CounterAttackExplo,
        Ipris_PatternOneAttack,
        Ipris_PatternTwo,
        Ipris_PatternTwoWindMill,
        Ipris_BackPos,
        Ipris_AttackFirst,
        Ipris_AttackSecond,
        Ipris_ToDragon,
        // ------------------------ Dragon ------------------------------
        Dragon_AttackDown,
        Dragon_AttackSwing,
        Dragon_AttackTail,
        Dragon_Fear,
        Dragon_BreathEnable,
        Dragon_BreathEnableHit,
        Dragon_BreathGroggy,
        Dragon_SkyDown,
        Dragon_GroundToSky,
        Dragon_Fireball,
        Dragon_FireballExplo,
        Dragon_FireballField,
        Dragon_Shield,
        // ------------------------ Dragon ------------------------------
        MaxCount
    }
}

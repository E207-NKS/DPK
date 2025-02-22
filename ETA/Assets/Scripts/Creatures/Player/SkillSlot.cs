using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public SkillSystem SkillSystem { get; set; }
    // Start is called before the first frame update
    //ISkill[] skill = new ISkill[8];
    public Skill[] skill = new Skill[8];
    public Skill[] Skills { get { return skill; } }

    private Animator _animator;

    private Skill _currentSkill;
    public Skill PreviousSkill;
    public Skill CurrentSkill { get { return _currentSkill; } }

    private string[] loadedSkills = new string[8];
    public string[] LoadedSkills { get { return loadedSkills; } }

    public int LoadCnt = 0; 

    public void Start()
    {
        SkillSystem = GetComponent<SkillSystem>();
        // 내 캐릭터만 UI에 연결하기


        if (GetComponent<PhotonView>().IsMine)
        {
            switch (gameObject.name.Replace("(Clone)", ""))
            {
                case "Warrior": // 워리어
                    //loadedSkills = new string[] { "DoubleSlash", "TripleSlash", "DrawSword", "QuadrupleSlash", "Guard", "Sting", "ShieldSlam", "BackStep" };
                    for (int i = 0; i < 8; i++)
                    {
                        if(Managers.Player.warriorSkills[i] == null)
                        {
                            loadedSkills[i] = "";
                        }
                        else
                        {
                            loadedSkills[i] = Managers.Player.warriorSkills[i].skillName;
                        }
                        
                    }
                    break;
                case "Archer": // 아처
                    //loadedSkills = new string[] { "StormStrike", "WindSpirit", "ArrowBomb", "WindBall", "WindShield", "ArrowStab", "ForestSpirit", "LightningShot" };
                    for (int i = 0; i < 8; i++)
                    {
                        if (Managers.Player.archerSkills[i] == null)
                        {
                            loadedSkills[i] = "";
                        }
                        else
                        {
                            loadedSkills[i] = Managers.Player.archerSkills[i].skillName;
                        }
                    }
                    break;
                case "Mage": // 메이지
                    //loadedSkills = new string[] { "Meteor", "ChainLightning", "FlashLight", "Thunder", "Heal", "Protection", "BloodBoom", "Gravity" };
                    for (int i = 0; i < 8; i++)
                    {
                        if (Managers.Player.mageSkills[i] == null)
                        {
                            loadedSkills[i] = "";
                        }
                        else
                        {
                            loadedSkills[i] = Managers.Player.mageSkills[i].skillName;
                        }
                    }
                    break;
            }

            for (int i = 0; i < loadedSkills.Length; i++)
            {
                string skillName = loadedSkills[i];
                Debug.Log(skillName);
                if (skillName == null || skillName == "") continue;
                Type type = Type.GetType(skillName);

                // Type이 유효하면 컴포넌트를 추가합니다.
                // 후에 as를 이용한 타입캐스트 해주기
                if (type != null && type.IsSubclassOf(typeof(Component)))
                {
                    skill[i] = (Skill)gameObject.AddComponent(type);

                }
            }

            FindObjectOfType<GameSystem>().LoadedSkillCnt += 1;
            //SendSkillsInfo(loadedSkills);

        }
        else
        {

        }

        


        if (GetComponent<PhotonView>().IsMine)
        {
            GameObject.FindObjectOfType<Dungeon_Popup_UI>().skillSlot = this;
            GameObject.FindObjectOfType<Dungeon_Popup_UI>().UpdateSlotSkillIcons();
        }
    }

    public void SendSkillsInfo()
    {
        string[] skillsInfo = new string[8];
        switch (Managers.Player.GetClassCode())
        {
            case "C001":
                for (int i = 0; i < 8; i++)
                {
                    if (Managers.Player.warriorSkills[i] == null)
                    {
                        skillsInfo[i] = "";
                    }
                    else
                    {
                        skillsInfo[i] = Managers.Player.warriorSkills[i].skillName;
                    }
                }
                break;
            case "C002":
                for (int i = 0; i < 8; i++)
                {
                    if (Managers.Player.archerSkills[i] == null)
                    {
                        skillsInfo[i] = "";
                    }
                    else
                    {
                        skillsInfo[i] = Managers.Player.archerSkills[i].skillName;
                    }
                }
                break;
            case "C003":
                for (int i = 0; i < 8; i++)
                {
                    if (Managers.Player.mageSkills[i] == null)
                    {
                        skillsInfo[i] = "";
                    }
                    else
                    {
                        skillsInfo[i] = Managers.Player.mageSkills[i].skillName;
                    }
                }
                break;
            default:
                Debug.LogError("없는 직업 입니다.");
                break;


        }
        
        

        GetComponent<PhotonView>().RPC("RPC_SendSkillsInfo", RpcTarget.Others, (object)skillsInfo);
    }



    [PunRPC]
    void RPC_SendSkillsInfo(string[] skillInfos)
    {
        for (int i = 0; i < skillInfos.Length; i++)
        {
            string skillName = skillInfos[i];
            Debug.Log(skillName);
            if (skillName == null) continue;
            Type type = Type.GetType(skillName);

            // Type이 유효하면 컴포넌트를 추가합니다.
            // 후에 as를 이용한 타입캐스트 해주기
            if (type != null && type.IsSubclassOf(typeof(Component)))
            {
                skill[i] = (Skill)gameObject.AddComponent(type);

            }
        }

        FindObjectOfType<GameSystem>().LoadedSkillCnt += 1;
        
    }


    public void SelectSkill(Define.SkillKey key)
    {
        Skill current = skill[(int)key];
        if (current == null) return;
        if (FindObjectOfType<GameSystem>().LoadedSkillCnt < PhotonNetwork.CurrentRoom.PlayerCount) return;
        // 여기서 쿨타임 관리
        if (current.CanCastSkill() == false) return;

        switch (current.SkillType)
        {
            case Define.SkillType.Target:
                SkillSystem.currentType = Define.SkillType.Target;
                SkillSystem.SkillRange = current.skillRange;
                break;
            case Define.SkillType.Range:
                SkillSystem.currentType = Define.SkillType.Range;
                SkillSystem.SkillRange = current.skillRange;
                SkillSystem.RangeType = current.RangeType;
                break;
            case Define.SkillType.Holding:
                SkillSystem.currentType = Define.SkillType.Holding;
                SkillSystem.SkillRange = current.skillRange;
                break;
            case Define.SkillType.Immediately:
                SkillSystem.currentType = Define.SkillType.Immediately;
                SkillSystem.SkillRange = current.skillRange;
                break;
        }


    }

    public void CancleSkill()
    {
        SkillSystem.currentType = Define.SkillType.None;

    }

    // 실제로 스테이트에서 발생하는 함수
    public void CastSkill(Define.SkillKey key)
    {
        //string s = skill[(int)key].animationName;
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }
        _currentSkill = skill[(int)key];
        _currentSkill.Cast();
        Debug.Log($"Skill Key = {key}");
    }

    public void CastCollavoSkill(Define.SkillKey key)
    {
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }
        _currentSkill = skill[(int)key];
        _currentSkill.CollavoCast();
    }

    public void NormalAttack()
    {
        // 이게 꼭 필요할까?
        if (_currentSkill != null)
        {
            _currentSkill.StopCast();
        }

        // TODO: 직업이 여러개면 바꿔 주어야할 것
        if (gameObject.name.Contains("Warrior"))
        {
            _currentSkill = gameObject.GetOrAddComponent<WarriorNormalAttackSkill>();
        }
        else if(gameObject.name.Contains("Archer"))
        {
            _currentSkill = gameObject.GetOrAddComponent<ArcherNormalAttackSkill>();
        }
        else if (gameObject.name.Contains("Mage"))
        {
            _currentSkill = gameObject.GetOrAddComponent<MageNormalAttackSkill>();
        }
        else
        {
            Debug.Log("존재하지 않는 직업 입니다.");
        }
        
        _currentSkill.Cast();
    }

    public void Clear()
    {
        Debug.Log("Skill System Cleared");
        SkillSystem.Clear();
    }
}
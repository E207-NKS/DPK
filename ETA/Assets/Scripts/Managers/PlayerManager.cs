using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private static PlayerManager instance;
    private string id;
    private string nickname;
    private string accessToken;
    private int gold;
    private string playerId;
    private string curClass = "C001";
    private int index;
    private bool partyLeader;
    private bool first;

    private int playerLevel = 1;
    private long curExp;
    private int skillPoint;

    public SkillInfo[] warriorSkills = new SkillInfo[8];
    public SkillInfo[] archerSkills = new SkillInfo[8];
    public SkillInfo[] mageSkills = new SkillInfo[8];

    // 워리어 아처 메이지
    public int[] playerAllLevel = new int[3];

    public PlayerManager() { }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }

        return instance;
    }

    public void init()
    {
        partyLeader = false;
        index = 0;
    }

    #region Setter
    public void SetId(string id)
    {
        this.id = id;
    }
    public void SetNickName(string nickname)
    {
        this.nickname = nickname;
    }
    public void SetToken(string accessToken)
    {
        this.accessToken = accessToken;
    }
    public void SetGold(int gold)
    {
        this.gold = gold;
    }
    
    public void SetPlayerId(string playerId)
    {
        this.playerId = playerId;
    }
    public void SetClassCode(string curClass)
    {
        this.curClass = curClass;
    }
    
    public void SetIndex(int index)
    {
        this.index = index;
    }
    
    public void SetPartyLeader(bool partyLeader)
    {
        this.partyLeader = partyLeader;
    }
    public void SetFirst(bool first)
    {
        this.first = first;
    }

    long CalculateExpRequirement(int level)
    {
        if (level >= 0 && level <= 5)
        {
            return 100; // 레벨 0~5까지는 각 레벨마다 100 경험치 필요
        }
        else if (level >= 6 && level <= 10)
        {
            return 500; // 레벨 6~10까지는 각 레벨마다 500 경험치 필요
        }
        else if (level >= 11 && level <= 20)
        {
            return 1000; // 레벨 11~20까지는 각 레벨마다 1000 경험치 필요
        }
        else
        {
            return 1000 + (level*100); // 범위 외 레벨에 대한 처리
        }
    }

    public void AddExp(long exp)
    {
        this.curExp += exp;

        long needExp = CalculateExpRequirement(playerLevel);
        //int needExp = 100 *(playerLevel/5);
        //(int)(100 * Math.Pow(5, playerLevel));
        Debug.Log("Cur Level : " + playerLevel);
        Debug.Log("Need Exp : " + needExp);
            Debug.Log("Cur Exp : " + curExp);
        while (curExp >= needExp)
        {
            if(curExp >= needExp)
            {
                curExp -= needExp;
                playerLevel++;
                needExp = CalculateExpRequirement(playerLevel);
                Managers.Photon.SetPlayerLevel();
                Debug.Log("Cur Level : " + playerLevel);

                if (curClass == "C001") playerAllLevel[0] = playerLevel;
                else if (curClass == "C002") playerAllLevel[1] = playerLevel;
                else if (curClass == "C003") playerAllLevel[2] = playerLevel;
            }
            else
            {
                break;
            }
            
            Debug.Log("Need Exp : " + needExp);
            Debug.Log("Cur Exp : " + curExp);

            // 갱신
        }

        if (curExp < 0) curExp = 0;
    }
    public void SetExp(long exp)
    {
        this.curExp = exp;
    }
    
    public void SetSkillPoint(int skillPoint)
    {
        this.skillPoint = skillPoint;
    }
    public void SetLevel(int level)
    {
        this.playerLevel = level;
    }
    public void SetAllLevel(string classCode,int level)
    {
        //Debug.Log($"{classCode}의 레벨은 {level}");
        int index = 0;
        if(classCode == "C001") index = 0;
        else if(classCode == "C002") index = 1;
        else if(classCode == "C003") index = 2;

        Debug.Log($"{index}의 레벨은 {level}");
        this.playerAllLevel[index] = level;
    }

    #endregion


    #region Getter
    public string GetId()
    {
        return id;
    }
    public string GetNickName()
    {
        return nickname;
    }
    public string GetToken()
    {
        return accessToken;
    }
    public int GetGold()
    {
        return gold;
    }

    public string GetPlayerId()
    {
        return playerId;
    }
    public string GetClassCode()
    {
        if (curClass == null)
            curClass =  "C001";
        return curClass;
    }
    public int GetIndex()
    {
        return index;
    }
    public bool GetPartyLeader()
    {
        return partyLeader;
    }
    public bool GetFirst()
    {
        return first;
    }

    public long GetExp()
    {
        return curExp;
    }
    public long GetNeedExp()
    {
        return CalculateExpRequirement(playerLevel); ;
    }
    public int GetLevel()
    {
        return playerLevel;
    }
    public int GetSkillPoint()
    {
        return skillPoint;
    }

    public int GetWarriorLevel()
    {
        return playerAllLevel[0];
    }
    public int GetArcherLevel()
    {
        return playerAllLevel[1];
    }
    public int GetMageLevel()
    {
        return playerAllLevel[2];
    }

    #endregion
}

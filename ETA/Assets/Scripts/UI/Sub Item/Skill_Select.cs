using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class Skill_Select : MonoBehaviour
{
    // ------------------------------ 변수 정의 ------------------------------

    // --------------- 스킬 목록 ---------------

    // 스킬 목록 정보
    private Button[] skillContainers = new Button[16]; // 스킬 버튼
    private Image[] skillIcons = new Image[16]; // 스킬 아이콘
    private Image[] skillCanNotBeUsedIcons = new Image[16]; // 스킬 비활성화 아이콘
    private TMP_Text[] skillKoreanNames = new TMP_Text[16]; // 스킬 한글 이름
    private TMP_Text[] skillRequiredLevels = new TMP_Text[16]; // 필요 레벨
    private TMP_Text[] skillDetail = new TMP_Text[16]; // 자세히 보기


    // --------------- 스킬 슬롯 ---------------

    // 임시 저장할 스킬 슬롯
    public SkillInfo[] tempWarriorSkills;
    public SkillInfo[] tempArcherSkills;
    public SkillInfo[] tempMageSkills;

    // 슬롯에 저장된 스킬 이미지
    private Image[] slotSkillIcons = new Image[8];

    // 현재 슬롯 번호. 초기값은 -1로 설정하여 어떤 슬롯에도 들어있지 않음을 의미
    public int currentSlotIndex = -1;

    // 스킬 영어 이름 배열
    private string[] skillNames = new string[16];

    // 스킬 코드 배열
    private string[] skillCodes = new string[16];

    // 드래그 중인 스킬 아이콘 이미지와 영어 이름을 보관할 클래스
    public class DraggingSkill
    {
        public Image Icon { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    // 드래그 중인 스킬 정보를 보관할 객체
    private DraggingSkill draggingSkill = new();


    // --------------- 캐릭터 정보 ---------------

    // 선택된 캐릭터 코드
    private string selectedClassCode;

    // 선택된 캐릭터 코드 이전 값
    private string previousSelectedClassCode;

    // 클래스 코드와 스킬 정보(SkillInfo 객체) 배열을 매핑하는 딕셔너리
    Dictionary<string, SkillInfo[]> classSkills = new()
    {
        {"C001", new SkillInfo[]
            {
                new() { skillCode = "W302", skillName = "DrawSword" },
                new() { skillCode = "W306", skillName = "DoubleSlash" },
                new() { skillCode = "W204", skillName = "HolyHammer" },
                new() { skillCode = "W309", skillName = "BackStep" },
                new() { skillCode = "W301", skillName = "Guard" },
                new() { skillCode = "W401", skillName = "WhirlWind" },
                new() { skillCode = "W402", skillName = "WindSlash" },
                new() { skillCode = "W308", skillName = "ShieldSlam" },
                new() { skillCode = "W104", skillName = "ThrowingShield" },
                new() { skillCode = "W307", skillName = "TripleSlash" },
                new() { skillCode = "W103", skillName = "Sting" },
                new() { skillCode = "W102", skillName = "SwordWave" },
                new() { skillCode = "W303", skillName = "Massacre" },
                new() { skillCode = "W305", skillName = "Blessing" },
                new() { skillCode = "W310", skillName = "QuadrupleSlash" },
                new() { skillCode = "W105", skillName = "MoonSword" },
            }
        },
        {"C002", new SkillInfo[]
            {
                new() { skillCode = "A301", skillName = "ArrowStab" },
                new() { skillCode = "A303", skillName = "ScatterArrow" },
                new() { skillCode = "A304", skillName = "Tumbling" },
                new() { skillCode = "A305", skillName = "WindBlast" },
                new() { skillCode = "A302", skillName = "WindShield" },
                new() { skillCode = "A401", skillName = "WindBall" },
                new() { skillCode = "A402", skillName = "LightningShot" },
                new() { skillCode = "A309", skillName = "ForestSpirit" },
                new() { skillCode = "A202", skillName = "ArrowBomb" },
                new() { skillCode = "A101", skillName = "RapidArrow" },
                new() { skillCode = "A308", skillName = "Tempest" },
                new() { skillCode = "A201", skillName = "ArrowShower" },
                new() { skillCode = "A307", skillName = "ForestBless" },
                new() { skillCode = "A306", skillName = "ChargeArrow" },
                new() { skillCode = "A310", skillName = "WindSpirit" },
                new() { skillCode = "A102", skillName = "StormStrike" },
            }
        },
        {"C003", new SkillInfo[]
             {
                new() { skillCode = "M306", skillName = "Teleport" },
                new() { skillCode = "M101", skillName = "BubbleShot" },
                new() { skillCode = "M201", skillName = "IceBone" },
                new() { skillCode = "M303", skillName = "FlashLight" },
                new() { skillCode = "M302", skillName = "Protection" },
                new() { skillCode = "M402", skillName = "Thunder" },
                new() { skillCode = "M401", skillName = "Gravity" },
                new() { skillCode = "M205", skillName = "FireWall" },
                new() { skillCode = "M203", skillName = "BloodBoom" },
                new() { skillCode = "M301", skillName = "Heal" },
                new() { skillCode = "M102", skillName = "Fear" },
                new() { skillCode = "M204", skillName = "PoisonMist" },
                new() { skillCode = "M304", skillName = "QuickFreeze" },
                new() { skillCode = "M305", skillName = "ChainLightning" },
                new() { skillCode = "M206", skillName = "Plague" },
                new() { skillCode = "M202", skillName = "Meteor" },
            }
        }
    };


    // ------------------------------ UI 초기화 ------------------------------

    private void Start()
    {
        // --------------- 캐릭터 정보 초기화 ---------------

        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // 선택된 캐릭터 코드의 초기 이전 값 설정
        previousSelectedClassCode = selectedClassCode;


        // --------------- 스킬 목록 ---------------

        // 스킬 목록 정보 연결
        for (int i = 0; i < 16; i++)
        {
            int index = i;
            skillContainers[index] = GameObject.Find($"Skill_{index + 1}_Container").GetComponent<Button>();
            skillIcons[index] = GameObject.Find($"Skill_{index + 1}_Icon").GetComponent<Image>();
            skillCanNotBeUsedIcons[index] = GameObject.Find($"Skill_{index + 1}_Can_Not_Be_Used").GetComponent<Image>();
            skillKoreanNames[index] = GameObject.Find($"Skill_{index + 1}_Name").GetComponent<TMP_Text>();
            skillRequiredLevels[index] = GameObject.Find($"Skill_{index + 1}_Required_Level").GetComponent<TMP_Text>();
            skillDetail[index] = GameObject.Find($"Skill_{index + 1}_Detail").GetComponent<TMP_Text>();

            // 드래그 이벤트 할당
            UI_Base.AddUIEvent(skillIcons[index].gameObject, (eventData) => OnDragBegin(index, eventData), Define.UIEvent.BeginDrag);
            UI_Base.AddUIEvent(skillIcons[index].gameObject, OnDrag, Define.UIEvent.Drag);
            UI_Base.AddUIEvent(skillIcons[index].gameObject, (eventData) => OnDragEnd(selectedClassCode, eventData), Define.UIEvent.EndDrag);
        }


        // --------------- 스킬 슬롯 ---------------

        // 스킬 슬롯 정보 연결
        for (int i = 0; i < 8; i++)
        {
            int index = i;
            slotSkillIcons[index] = GameObject.Find($"Slot_Skill_{index + 1}").GetComponent<Image>();

            // UI 이벤트를 추가하기 위해 EventTrigger 컴포넌트를 가져옵니다.
            EventTrigger eventTrigger = slotSkillIcons[index].gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                // EventTrigger 컴포넌트가 없다면 추가합니다.
                eventTrigger = slotSkillIcons[index].gameObject.AddComponent<EventTrigger>();
            }

            // 포인터 진입 이벤트를 추가
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => { OnPointerEnterSlot(index); });
            eventTrigger.triggers.Add(entryEnter);

            // 포인터 나감 이벤트를 추가
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => { OnPointerExitSlot(); });
            eventTrigger.triggers.Add(entryExit);

            // 포인터 클릭 이벤트를 추가
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerClick;
            entryClick.callback.AddListener((data) => { OnPointerClickSlot((PointerEventData)data, index, selectedClassCode); });
            eventTrigger.triggers.Add(entryClick);
        }


        // --------------- 드래그 중인 스킬 아이콘 ---------------

        draggingSkill.Icon = new GameObject("DraggingIcon").AddComponent<Image>();
        draggingSkill.Icon.transform.SetParent(transform, false);
        draggingSkill.Icon.rectTransform.sizeDelta = new Vector2(65, 65); // 아이콘 크기 설정
        draggingSkill.Icon.gameObject.SetActive(false); // 시작 시 비활성화


        // --------------- 스킬 정보 업데이트---------------

        // 선택한 스킬 정보 업데이트
        UpdateSelectedSkillInfo(selectedClassCode);

        // 스킬 슬롯 업데이트
        GetSkillSlot(selectedClassCode);

        // 모든 스킬 정보 업데이트
        UpdateAllSkillInfo(selectedClassCode);
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    private void Update()
    {
        // 선택된 캐릭터 코드 초기화
        selectedClassCode = Character_Select.selectedClassCode;

        // selectedClassCode 값이 이전 값과 다를 경우
        if (selectedClassCode != previousSelectedClassCode)
        {
            // 이전 selectedClassCode 값을 현재 값으로 업데이트
            previousSelectedClassCode = selectedClassCode;

            // 선택한 스킬 정보 업데이트
            UpdateSelectedSkillInfo(selectedClassCode);

            // 스킬 슬롯 업데이트
            GetSkillSlot(selectedClassCode);

            // 모든 스킬 정보 업데이트
            UpdateAllSkillInfo(selectedClassCode);
        }
    }


    // ------------------------------ 메서드 정의 ------------------------------

    // 클래스별 스킬 슬롯 업데이트 메서드
    private void GetSkillSlot(string classCode)
    {
        switch (classCode)
        {
            case "C001":
                tempWarriorSkills = DeepCopySkillSlot(Managers.Player.warriorSkills);
                break;
            case "C002":
                tempArcherSkills = DeepCopySkillSlot(Managers.Player.archerSkills);
                break;
            case "C003":
                tempMageSkills = DeepCopySkillSlot(Managers.Player.mageSkills);
                break;
            default:
                // 기본값 또는 예외 처리
                break;
        }
    }

    // 깊은 복사 메서드
    public SkillInfo[] DeepCopySkillSlot(SkillInfo[] original)
    {
        if (original == null)
            return null;

        SkillInfo[] copy = new SkillInfo[original.Length];

        for (int i = 0; i < original.Length; i++)
        {
            if (original[i] != null)
            {
                copy[i] = new SkillInfo { skillCode = original[i].skillCode, skillName = original[i].skillName };
            }
            else
            {
                copy[i] = null;
            }
        }
        return copy;
    }

    // 모든 스킬 정보를 업데이트하는 메서드
    private void UpdateAllSkillInfo(string classCode)
    {
        // classCode가 null이 아닌지 확인
        if (classCode == null)
        {
            Debug.Log("classCode는 null일 수 없습니다.");
            return;
        }

        // 스킬 목록 업데이트
        UpdateSkillList(classCode);

        // 스킬 슬롯 업데이트
        UpdateSkillSlot(classCode);
    }

    // 스킬 목록 업데이트 메서드
    private void UpdateSkillList(string classCode)
    {
        // 클래스별 레벨 가져오기
        int classLevel = classCode switch
        {
            "C001" => Managers.Player.GetWarriorLevel(),
            "C002" => Managers.Player.GetArcherLevel(),
            "C003" => Managers.Player.GetMageLevel(),
            _ => 0
        };

        for (int i = 0; i < 16; i++)
        {
            try
            {
                // SkillSO 객체 로드
                SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{ChangeCodeToName(classCode)}/{classSkills[classCode][i].skillName}");

                // SkillSO 값이 null일 경우 스킬 칸을 비우고 반복문 넘어감
                if (skillData == null)
                {
                    skillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
                    skillCanNotBeUsedIcons[i].gameObject.SetActive(true);
                    skillKoreanNames[i].text = "";
                    skillRequiredLevels[i].text = "";
                    skillDetail[i].text = "";
                    continue;
                }

                // 스킬 정보 업데이트
                skillIcons[i].sprite = skillData.Icon;
                skillKoreanNames[i].text = skillData.SkillKoreanName;
                skillRequiredLevels[i].text = $"필요레벨 {skillData.RequiredLevel}";
                skillDetail[i].text = $"상세 보기 →";

                // 영어 이름을 배열에 추가
                skillNames[i] = skillData.SkillName;

                // 스킬 코드를 배열에 추가
                skillCodes[i] = skillData.SkillCode;

                // 필요 레벨이 플레이어 레벨보다 클 경우 텍스트 색상을 빨간색으로, 그렇지 않으면 흰색으로 설정
                if (skillData.RequiredLevel > classLevel)
                {
                    skillCanNotBeUsedIcons[i].gameObject.SetActive(true);
                    skillKoreanNames[i].color = new Color(1, 0, 0, 0.8f);
                    skillRequiredLevels[i].color = new Color(1, 0, 0, 0.8f);
                    skillDetail[i].color = new Color(1, 0, 0, 0.8f);
                }
                else
                {
                    skillCanNotBeUsedIcons[i].gameObject.SetActive(false);
                    skillKoreanNames[i].color = Color.white;
                    skillRequiredLevels[i].color = new Color(1, 1, 1, 0.6f);
                    skillDetail[i].color = new Color(1, 1, 1, 0.6f);
                }
            }
            catch (Exception e)
            {
                // SkillSO가 없을 경우 기본 이미지 할당
                Debug.Log("Error loading skill data: " + e.Message);
                skillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
            }
        }
    }

    // 스킬 슬롯 아이콘 업데이트 메서드
    public void UpdateSkillSlot(string classCode)
    {
        SkillInfo[] skills = classCode switch
        {
            "C001" => tempWarriorSkills,
            "C002" => tempArcherSkills,
            "C003" => tempMageSkills,
            _ => null
        };

        for (int i = 0; i < 8; i++)
        {
            try
            {
                // SkillSO 객체 로드
                SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{ChangeCodeToName(classCode)}/{skills[i].skillName}");

                if (skillData != null)
                {
                    slotSkillIcons[i].sprite = skillData.Icon;
                }
                else
                {
                    slotSkillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
                }
            }
            catch (Exception e)
            {
                // SkillSO가 없을 경우 기본 이미지 할당
                Debug.Log("Error loading skill data: " + e.Message);
                slotSkillIcons[i].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
            }
        }
    }

    // 선택한 스킬 정보 업데이트 메서드
    public void UpdateSelectedSkillInfo(string classCode)
    {
        if (classCode == null || !classSkills.ContainsKey(classCode))
        {
            Debug.Log("Invalid classCode: " + classCode);
            return;
        }

        // Skill_Info 컴포넌트 참조
        Skill_Info skillInfoComponent = GameObject.Find("Skill_Info").GetComponent<Skill_Info>();

        // 스킬 목록 정보 연결
        for (int i = 0; i < 16; i++)
        {
            // 클래스 코드에 따른 스킬 이름 가져오기
            string skillName = classSkills[classCode][i].skillName;

            // 기존 클릭 이벤트 리스너를 모두 제거
            skillContainers[i].onClick.RemoveAllListeners();

            // 버튼 클릭 이벤트 추가
            skillContainers[i].onClick.AddListener(() => skillInfoComponent.UpdateSkillInfo(ChangeCodeToName(classCode), skillName));
        }
    }

    // 클래스 코드로 클래스 이름을 string으로 반환하는 메서드
    public static string ChangeCodeToName(string classCode)
    {
        string className = classCode switch
        {
            "C001" => "Warrior",
            "C002" => "Archer",
            "C003" => "Mage",
            _ => null
        };
        return className;
    }

    // 드래그 시작 메서드
    public void OnDragBegin(int skillIndex, PointerEventData eventData)
    {
        // 드래그 시작 시 드래그 중인 스킬 정보를 활성화하고 해당 슬롯의 이미지와 이름을 복제
        draggingSkill.Icon.sprite = skillIcons[skillIndex].sprite;
        draggingSkill.Name = classSkills[selectedClassCode][skillIndex].skillName;
        draggingSkill.Code = classSkills[selectedClassCode][skillIndex].skillCode;
        draggingSkill.Icon.gameObject.SetActive(true);

        // 스프라이트의 투명도를 50%로 설정
        Color tempColor = draggingSkill.Icon.color;
        tempColor.a = 0.5f;
        draggingSkill.Icon.color = tempColor;

        // 드래그 중인 스킬 아이콘의 Raycast Target을 비활성화
        draggingSkill.Icon.raycastTarget = false;
    }

    // 드래그 종료 메서드
    public void OnDragEnd(string classCode, PointerEventData eventData)
    {
        SkillInfo[] skills = classCode switch
        {
            "C001" => tempWarriorSkills,
            "C002" => tempArcherSkills,
            "C003" => tempMageSkills,
            _ => null
        };

        // 드래그 종료 시 드래그 중인 스킬 아이콘의 Raycast Target을 활성화
        draggingSkill.Icon.raycastTarget = true;

        // 드롭 이벤트 발생 시 드래그 중인 스킬 정보를 해당 슬롯에 업데이트
        if (draggingSkill != null && draggingSkill.Icon.gameObject.activeSelf)
        {
            // 해당 슬롯 인덱스가 유효하면 업데이트
            if (currentSlotIndex != -1)
            {
                // 같은 이름의 스킬이 있는지 확인
                bool isDuplicate = false;
                for (int i = 0; i < skills.Length; i++)
                {
                    // skills[i]가 null이 아닌지 그리고 스킬 이름이 같은지 확인
                    if (skills[i] != null && skills[i].skillName == draggingSkill.Name)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                // 중복된 스킬을 슬롯에 장착하려고 하면 경고 창 출력
                if (isDuplicate)
                {
                    // 경고창 띄우기
                    var popupUI = FindFirstObjectByType<Lobby_Popup_UI>();
                    Managers.Coroutine.StartCoroutine(popupUI.ShowWarningPopupCoroutine("스킬 장착 불가", "동일한 스킬을 슬롯에 장착할 수 없습니다."));
                }
                else
                {
                    try
                    {
                        // 스킬 슬롯 업데이트
                        skills[currentSlotIndex] = new SkillInfo { skillCode = draggingSkill.Code, skillName = draggingSkill.Name };

                        // SkillSO 객체 로드
                        SkillSO skillData = Resources.Load<SkillSO>($"Scriptable/Skill/{ChangeCodeToName(selectedClassCode)}/{skills[currentSlotIndex].skillName}");

                        if (skillData != null)
                        {
                            slotSkillIcons[currentSlotIndex].sprite = skillData.Icon;
                        }
                        else
                        {
                            slotSkillIcons[currentSlotIndex].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
                        }
                    }
                    catch (Exception e)
                    {
                        // SkillSO가 없을 경우 기본 이미지 할당
                        Debug.Log("Error loading skill data: " + e.Message);
                        slotSkillIcons[currentSlotIndex].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
                    }
                }
            }
        }

        // 드래그 종료 시 드래그 중인 스킬 아이콘을 비활성화
        draggingSkill.Icon.gameObject.SetActive(false);
    }

    // 드래그 중 메서드
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중인 스킬 아이콘의 위치를 마우스 커서 위치로 업데이트
        draggingSkill.Icon.transform.position = eventData.position;
    }

    // 포인터가 슬롯에 진입했을 때
    public void OnPointerEnterSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex;
    }

    // 포인터가 슬롯에서 나갔을 때
    public void OnPointerExitSlot()
    {
        currentSlotIndex = -1;
    }

    // 포인터 클릭 이벤트 핸들러
    public void OnPointerClickSlot(PointerEventData eventData, int index, string classCode)
    {
        SkillInfo[] skills = classCode switch
        {
            "C001" => tempWarriorSkills,
            "C002" => tempArcherSkills,
            "C003" => tempMageSkills,
            _ => null
        };

        // 우클릭 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 스킬 정보 비우기
            if (skills.Length > index)
            {
                skills[index] = null; // 스킬 정보를 null로 설정
                slotSkillIcons[index].sprite = Resources.Load<Sprite>("Sprites/Skill Slot/Skill Cooldown Image");
            }
        }
        // 좌클릭 확인
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 스킬 정보 띄우기
            if (skills.Length > index && skills[index] != null) // 스킬 배열 범위 확인 및 스킬 정보가 null이 아닌지 확인
            {
                // Skill_Info 컴포넌트 참조
                GameObject skillInfoObject = GameObject.Find("Skill_Info");
                if (skillInfoObject != null) // skillInfoObject가 null이 아닌지 확인
                {
                    Skill_Info skillInfoComponent = skillInfoObject.GetComponent<Skill_Info>();
                    if (skillInfoComponent != null) // skillInfoComponent가 null이 아닌지 확인
                    {
                        skillInfoComponent.UpdateSkillInfo(ChangeCodeToName(selectedClassCode), skills[index].skillName);
                    }
                    else
                    {
                        Debug.LogError("Skill_Info 컴포넌트를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.LogError("Skill_Info 게임 오브젝트를 찾을 수 없습니다.");
                }
            }
        }
    }

}

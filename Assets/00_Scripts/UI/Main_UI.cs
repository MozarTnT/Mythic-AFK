using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null;

    #region Parameter
    [Header("##Default")]
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_ALLATK_Text;
    [SerializeField] private TextMeshProUGUI m_LevelUp_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Stage_Count_Text;
    [SerializeField] private TextMeshProUGUI m_Stage_Text;

    Color m_Stage_Color = new Color(0, 0.7295136f, 1.0f, 1.0f);

    [Space(20.0f)]
    [Header("##Fade")]
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;

    [Space(20.0f)]
    [Header("##Monster_Slider")]
    [SerializeField] private GameObject m_Monster_Slider_OBJ;
    [SerializeField] private Image m_Monster_Slider;
    [SerializeField] private TextMeshProUGUI m_Monster_Value_Text;

    [Space(20.0f)]
    [Header("##Boss_Slider")]
    [SerializeField] private GameObject m_Boss_Slider_OBJ;
    [SerializeField] private Image m_Boss_Slider_Image;
    [SerializeField] private TextMeshProUGUI m_Boss_Value_Text, m_Boss_Stage_Text;

    [Space(20.0f)]
    [Header("##Dead_Frame")]
    [SerializeField] private GameObject Dead_Frame;

    [Space(20.0f)]
    [Header("##Legendary_PopUP")]
    [SerializeField] private Animator m_Legendary_PopUP;
    [SerializeField] private Image m_Item_Frame;
    [SerializeField] private Image m_PopUp_Image;
    [SerializeField] private TextMeshProUGUI m_PopUp_Text;
    Coroutine Legendary_Coroutine;
    
    bool isPopUP = false;

    [Space(20.0f)]
    [Header("##Item_PopUP")]
    [SerializeField] private Transform m_ItemContent;
    private List<TextMeshProUGUI> m_Item_Texts = new List<TextMeshProUGUI>();
    private List<Coroutine> m_Item_Coroutines = new List<Coroutine>();


    [Space(20.0f)]
    [Header("##Hero_Frame")]
    [SerializeField] private UI_Main_Part[] m_Main_Parts;
    public Image Main_Character_Skill_Fill;    
    Dictionary<Player, UI_Main_Part> m_Part = new Dictionary<Player, UI_Main_Part>();


    [Header("## ADS")]
    [SerializeField] private Image Fast_Lock;
    [SerializeField] private GameObject Fast_Fade;
    [SerializeField] private GameObject[] Buffs_Lock;
    [SerializeField] private Image x2Fill;
    [SerializeField] private TextMeshProUGUI x2Text;
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

    }

    private void Start()
    {
        TextCheck();
        Monster_Slider_Count();

        Base_Manager.isFast = PlayerPrefs.GetInt("FAST") == 1 ? true : false; // 앞서 저장된 배속 상태 가져오기
        TimeCheck();
        BuffCheck();

        for(int i = 0; i < m_ItemContent.childCount; i++)
        { 
            m_Item_Texts.Add(m_ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            m_Item_Coroutines.Add(null);
        }

        Stage_Manager.m_ReadyEvent += OnReady;
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;

        Stage_Manager.State_Change(Stage_State.Ready);
    }

    private void Update()
    {
        if(Data_Manager.m_Data.Buff_x2 > 0.0f)
        {
            x2Fill.fillAmount = Data_Manager.m_Data.Buff_x2 / 1800.0f;
            x2Text.text = Utils.GetTimer(Data_Manager.m_Data.Buff_x2);
        }
    }


   

    public void BuffCheck()
    {
        for(int i = 0; i < Data_Manager.m_Data.Buff_Timers.Length; i++)
        {
            if(Data_Manager.m_Data.Buff_Timers[i] > 0.0f)
            {
                Buffs_Lock[i].SetActive(false);

            }
            else
            {
                Buffs_Lock[i].SetActive(true);
            }
        }
        if(Data_Manager.m_Data.Buff_x2 > 0.0f)
        {
            x2Fill.transform.parent.gameObject.SetActive(true);
        }

        else
        {
            x2Fill.transform.parent.gameObject.SetActive(false);
        }
    }
    private void TimeCheck() // 게임 속도 조정 (1.0f : 1초, 1.5f : 0.5초 -> 1.5배로 줄어들어서 빠르게 진행)
    {
        Time.timeScale = Base_Manager.isFast ? 1.5f : 1.0f;
        Fast_Lock.gameObject.SetActive(!Base_Manager.isFast);
        Fast_Fade.SetActive(Base_Manager.isFast);
    }
    public void GetFast()
    {
        bool fast = !Base_Manager.isFast;
        if(fast == true)
        {
            if(Data_Manager.m_Data.Buff_x2 <= 0.0f)
            {
                Base_Manager.ADS.ShowRewardedAds(() => 

                {
                    Data_Manager.m_Data.Buff_x2 = 1800.0f;
                    BuffCheck();
                    TimeCheck();
                });

            }

        }

        Base_Manager.isFast = fast;
        PlayerPrefs.SetInt("FAST", fast == true ? 1 : 0); // 배속 상태 저장

        BuffCheck();
        TimeCheck();

    }


    public void GetItem(Item_Scriptable item)
    {
        bool AllActive = true;

        for(int i = 0; i < m_Item_Texts.Count; i++)
        {
            if(m_Item_Texts[i].gameObject.activeSelf == false) // 비활성화일때
            {
                m_Item_Texts[i].gameObject.SetActive(true);
                m_Item_Texts[i].text = "아이템을 획득하였습니다 : " + Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>";
                
                for(int j = 0; j < i; j++) // 먼저 나온 오브젝트 위로 이동
                {
                    RectTransform rect = m_Item_Texts[j].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 50.0f);
                }

                if(m_Item_Coroutines[i] != null)
                {
                    StopCoroutine(m_Item_Coroutines[i]);
                }
                m_Item_Coroutines[i] = StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                AllActive = false;
                break;
            }
        }

        if(AllActive) // 모든 오브젝트가 활성화 상태라면
        {
            GameObject BaseRect = null;
            float yCount = 0.0f;
            for(int i = 0; i < m_Item_Texts.Count; i++)
            {
                RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                if(rect.anchoredPosition.y > yCount)
                {
                    BaseRect = rect.gameObject;
                    yCount = rect.anchoredPosition.y;
                }
            }

            for(int i = 0; i < m_Item_Texts.Count; i++)
            {
                if (BaseRect == m_Item_Texts[i].gameObject)
                {
                    m_Item_Texts[i].gameObject.SetActive(false);
                    m_Item_Texts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

                    m_Item_Texts[i].gameObject.SetActive(true);
                    m_Item_Texts[i].text = "아이템을 획득하였습니다 : " + Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>";

                    if(m_Item_Coroutines[i] != null)
                    {
                        StopCoroutine(m_Item_Coroutines[i]);
                    }
                    m_Item_Coroutines[i] = StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                }
                else
                {
                    RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0.0f, rect.anchoredPosition.y + 50.0f);
                }
            }
        }

        if((int)item.rarity >= (int)Rarity.Hero) // 상단 애니메이션 노출
        {
            GetLegendaryPopUP(item);
        }

    }

    IEnumerator Item_Text_FadeOut(RectTransform rect)
    {
        yield return new WaitForSeconds(2.0f); // 추후 TimeScale 영향 받지 않게 해야함, WaitForSecondsRealtime(2.0f)
        rect.gameObject.SetActive(false);
        rect.anchoredPosition = new Vector2(0.0f, 0.0f);
    }



    public void Set_Boss_State()
    {
        Stage_Manager.isDead = false;
        Stage_Manager.State_Change(Stage_State.Boss);
    }


    private void SliderOBJCheck(bool Boss)
    {
        if(Stage_Manager.isDead)
        {
            m_Monster_Slider_OBJ.SetActive(false);
            m_Boss_Slider_OBJ.SetActive(false);

            Dead_Frame.SetActive(true);
            return;
        }

        Dead_Frame.SetActive(false);
        m_Monster_Slider_OBJ.SetActive(!Boss);
        m_Boss_Slider_OBJ.SetActive(Boss);

        Monster_Slider_Count();
        
        float value = Boss ? 1.0f : 0f;
        Boss_Slider_Count(value, 1.0f);
      
    }

   

    public void Set_Character_Data()
    {
        int indexValue = 0;
        for(int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if(data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, true);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
            }
        }

    }

    public void Character_State_Check(Player player)
    {
        // Dictionary에 키가 있는지 확인
        if (m_Part.ContainsKey(player))
        {
            m_Part[player].StateCheck(player);
        }
        else
        {
            Debug.LogWarning($"Player {player.name}가 UI Dictionary에 없습니다.");
            // Character_Spawner.players 배열에서 해당 플레이어를 찾아 Dictionary에 추가
            for (int i = 0; i < Character_Spawner.players.Length; i++)
            {
                if (Character_Spawner.players[i] == player)
                {
                    m_Part.Add(player, m_Main_Parts[i]);
                    m_Part[player].StateCheck(player);
                    break;
                }
            }
        }
    }

    public void Character_State_Check111(Player player)
    {
        m_Part[player].StateCheck(player);
    }
    
    private void OnReady()
    {
        FadeInOut(true);
        
        m_Part.Clear();
        
        for(int i = 0; i < 6; i++)
        {
            m_Main_Parts[i].Initialize();
        }

        int indexValue = 0;

        for(int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if(data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, false);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
                
                if (i < Character_Spawner.players.Length && Character_Spawner.players[i] != null)
                {
                    if (!m_Part.ContainsKey(Character_Spawner.players[i]))
                    {
                        m_Part.Add(Character_Spawner.players[i], m_Main_Parts[i]);
                    }
                }
                else
                {
                    Debug.LogWarning($"Player at index {i} is not ready yet");
                }
            }
        }
    }

    private void OnReady111()
    {
        FadeInOut(true);
        
        m_Part.Clear();
        
        for(int i = 0; i < 6; i ++)
        {
            m_Main_Parts[i].Initialize();
        }

        int indexValue = 0;

        for(int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Manager.Character.m_Set_Character[i];
            if(data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, false);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
                m_Part.Add(Character_Spawner.players[i], m_Main_Parts[i]);
            }
            else
            {
                Debug.Log("Player is null");
            }
        }
    }

    private void OnBoss()
    {
        TextCheck();
        SliderOBJCheck(true);
    }

    private void OnClear()
    {
        SliderOBJCheck(false);
        StartCoroutine(Clear_Delay());
    }

    private void OnDead()
    {
        TextCheck();
        StartCoroutine(Dead_Delay());
    }

    IEnumerator Dead_Delay()
    {
        yield return StartCoroutine(Clear_Delay());
        SliderOBJCheck(false);
        for(int i = 0; i < Spawner.m_Monsters.Count; i++)
        {
            if(Spawner.m_Monsters[i].isBoss == true)
            {
                Destroy(Spawner.m_Monsters[i].gameObject);
            }
            else
            {
                Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(Spawner.m_Monsters[i].gameObject);
            }
        }
        Spawner.m_Monsters.Clear();
    }

    IEnumerator Clear_Delay()
    {
        yield return new WaitForSeconds(2.0f);
        FadeInOut(false);

        yield return new WaitForSeconds(1.0f);
        Stage_Manager.State_Change(Stage_State.Ready);
    }

    public void Monster_Slider_Count()
    {
        float value = (float)Stage_Manager.Count / (float)Stage_Manager.MaxCount;

        if(value >= 1.0f)
        {
            value = 1.0f;
            if(Stage_Manager.m_State != Stage_State.Boss)
            {
                Stage_Manager.State_Change(Stage_State.Boss);
            }
        }
        m_Monster_Slider.fillAmount = value;
        m_Monster_Value_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }
    
    public void Boss_Slider_Count(double hp, double maxHP)
    {
        float value = (float)hp / (float)maxHP;

        if(value <= 0f)
        {
            value = 0f;
        }
        m_Boss_Slider_Image.fillAmount = value;
        m_Boss_Value_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }




    public void FadeInOut(bool FadeInOut, bool Sibling = false, Action action = null)
    {
        if(!Sibling) // FadeInOut을 전체 적용 및 UI 일부 적용에 따라 나눔
        {
            m_Fade.transform.parent = this.transform;
            m_Fade.transform.SetSiblingIndex(0);
        }
        else
        {
            m_Fade.transform.parent = Base_Canvas.instance.transform;
            m_Fade.transform.SetAsLastSibling();
        }

        StartCoroutine(FadeInOut_Coroutine(FadeInOut, action));
    }

    IEnumerator FadeInOut_Coroutine(bool FadeInOut, Action action = null)
    {
        if(FadeInOut == false)
        {
            m_Fade.raycastTarget = true;
        }

        float current = 0f;
        float percent = 0f;
        float start = FadeInOut ? 1.0f : 0f;
        float end = FadeInOut ? 0f : 1.0f;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / m_FadeDuration;

            float LerpPos = Mathf.Lerp(start, end, percent);
            m_Fade.color = new Color(0f, 0f, 0f, LerpPos);

            yield return null;
        }
   
        if(action != null) action?.Invoke();

        m_Fade.raycastTarget = false;
    }



    public void TextCheck()
    {
        m_Level_Text.text = "Lv." + (Data_Manager.m_Data.Level + 1).ToString();
        m_ALLATK_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_ATK());


        double LevelUpMoneyValue = Utils.Data.levelData.MONEY();

        m_LevelUp_Money_Text.text = StringMethod.ToCurrencyString(LevelUpMoneyValue);
        
        m_LevelUp_Money_Text.color = Utils.Coin_Check(LevelUpMoneyValue) ? Color.green : Color.red;

        m_Money_Text.text = StringMethod.ToCurrencyString(Data_Manager.m_Data.Money);


        m_Stage_Text.text = Stage_Manager.isDead ? "반복중..." : "진행중...";
        m_Stage_Text.color = Stage_Manager.isDead ? Color.yellow : m_Stage_Color;

        int stageValue = Data_Manager.m_Data.Stage + 1;
        int stageForward = (stageValue / 10) + 1;
        int stageBack = stageValue % 10;


        // 스테이지 / 10 + 1
        
        m_Stage_Count_Text.text = "보통 " + stageForward.ToString() + " - " + stageBack.ToString();

    }

    private void GetLegendaryPopUP(Item_Scriptable item)
    {
        if(isPopUP == true)
        {
            m_Legendary_PopUP.gameObject.SetActive(false);
        }
        isPopUP = true;
        m_Legendary_PopUP.gameObject.SetActive(true);

        m_Item_Frame.sprite = Utils.Get_Atlas(item.rarity.ToString());

        m_PopUp_Image.sprite = Utils.Get_Atlas(item.name);
        m_PopUp_Image.SetNativeSize();

        m_PopUp_Text.text = Utils.String_Color_Rarity(item.rarity) + item.Item_Name + "</color>을(를) 획득했습니다!";

        if(Legendary_Coroutine != null) // 코루틴 중복시 바로 삭제 방지
        {
            StopCoroutine(Legendary_Coroutine);
        }
        Legendary_Coroutine = StartCoroutine(Legendary_PopUP_Coroutine());
    }

    IEnumerator Legendary_PopUP_Coroutine()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        isPopUP = false;
        m_Legendary_PopUP.SetTrigger("Close");
    }

  
}

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

        for(int i = 0; i < m_ItemContent.childCount; i++)
        { 
            m_Item_Texts.Add(m_ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            m_Item_Coroutines.Add(null);
        }

        Stage_Manager.m_ReadyEvent += () => FadeInOut(true);
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;
    }

    [Header("##Default")]
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_ALLATK_Text;
    [SerializeField] private TextMeshProUGUI m_LevelUp_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Money_Text;

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
        yield return new WaitForSeconds(2.0f);
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

    private void OnBoss()
    {
        SliderOBJCheck(true);
    }

    private void OnClear()
    {
        SliderOBJCheck(false);
        StartCoroutine(Clear_Delay());
    }

    private void OnDead()
    {
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
        m_Level_Text.text = "Lv." + (Base_Manager.Data.Level + 1).ToString();
        m_ALLATK_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_ATK());

        double LevelUpMoneyValue = Utils.Data.levelData.MONEY();

        m_LevelUp_Money_Text.text = StringMethod.ToCurrencyString(LevelUpMoneyValue);
        
        m_LevelUp_Money_Text.color = Utils.Coin_Check(LevelUpMoneyValue) ? Color.green : Color.red;

        m_Money_Text.text = StringMethod.ToCurrencyString(Base_Manager.Data.Money);
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
        yield return new WaitForSeconds(2.0f);
        isPopUP = false;
        m_Legendary_PopUP.SetTrigger("Close");
    }

  
}

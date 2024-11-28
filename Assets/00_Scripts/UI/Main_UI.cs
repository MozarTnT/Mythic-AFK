using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        Stage_Manager.m_ReadyEvent += () => FadeInOut(true);
    }


    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_ALLATK_Text;

    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;
    [SerializeField] private Image m_Monster_Slider;
    [SerializeField] private TextMeshProUGUI m_Monster_Value_Text;

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
        m_Level_Text.text = "Lv." + (Base_Manager.Player.Level + 1).ToString();
        m_ALLATK_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_ATK());
    }
  
}

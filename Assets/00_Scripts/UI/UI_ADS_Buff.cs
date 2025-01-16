using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ADS_Buff : UI_Base
{
    public enum ADS_Buff_State{ ATK, GOLD, CRITICAL}
    
    [SerializeField] private TextMeshProUGUI m_LevelText, m_CountText;
    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private Image[] m_Buttons_Fill;
    [SerializeField] private Image m_Level_Fill;
    [SerializeField] private GameObject[] m_Buttons_Lock, m_Lock, m_ButtonFrame;
    [SerializeField] private TextMeshProUGUI[] m_Timer_Texts;

    public override bool Init()
    {
        for(int i = 0; i < Base_Manager.Data.Buff_Timers.Length; i++)
        {
            int index = i;
            m_Buttons[index].onClick.AddListener(() => GetBuff((ADS_Buff_State)index));
            if(Base_Manager.Data.Buff_Timers[i] > 0.0f)
            {
                SetBuff(i, true);
            }
        }

        return base.Init();
    }

    private void Update()
    {
        for(int i = 0; i < Base_Manager.Data.Buff_Timers.Length; i++)
        {
            if(Base_Manager.Data.Buff_Timers[i] >= 0.0f)
            {
                m_Buttons_Fill[i].fillAmount = 1 - (Base_Manager.Data.Buff_Timers[i] / 1800.0f);
                TimeSpan timespan = TimeSpan.FromSeconds(Base_Manager.Data.Buff_Timers[i]);
                string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);
                m_Timer_Texts[i].text = timer;
            }
        }
    }


    public void GetBuff(ADS_Buff_State m_State)
    {
        int stateValue = (int)m_State;

        Base_Manager.Data.BuffCount++;

        Base_Manager.Data.Buff_Timers[stateValue] = 1800.0f;
        Main_UI.instance.BuffCheck();

        SetBuff(stateValue, true);
    }

    void SetBuff(int value,bool GetBool)
    {
        m_Buttons_Lock[value].SetActive(GetBool);
        m_Lock[value].SetActive(!GetBool);
        m_ButtonFrame[value].SetActive(GetBool);
    }

}

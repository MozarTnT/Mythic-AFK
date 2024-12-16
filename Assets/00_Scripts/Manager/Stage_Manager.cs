using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();

// State Pattern -> Ready, Play, Boss, Clear, Dead
public class Stage_Manager 
{
    public static Stage_State m_State;
    public static int MaxCount = 3;
    public static int Count;

    public static bool isDead = false;

    public static OnReadyEvent m_ReadyEvent; // 델리게이트 체인 : 하나의 델리게이트가 여러 함수를 참조 가능함
    public static OnPlayEvent m_PlayEvent;
    public static OnBossEvent m_BossEvent;
    public static OnBossPlayEvent m_BossPlayEvent;
    public static OnClearEvent m_ClearEvent;
    public static OnDeadEvent m_DeadEvent;

    public static void State_Change(Stage_State state)
    {
        m_State = state;
        switch(state)
        {
            case Stage_State.Ready: 
                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Base_Manager.Data.Stage]["MaxCount"].ToString());
                Debug.Log("isReady");
                Debug.Log("MaxCount : " + MaxCount);
                m_ReadyEvent?.Invoke();
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                Debug.Log("isPlay");
                m_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Count = 0;
                Debug.Log("isBoss");
                m_BossEvent?.Invoke();
                break;
            case Stage_State.Boss_Play:
                Debug.Log("isBoss_Play");
                m_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                Base_Manager.Data.Stage++;
                Debug.Log("isClear");
                m_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                Debug.Log("isDead");
                isDead = true;
                m_DeadEvent?.Invoke();
                break;
        }
    }
   
}

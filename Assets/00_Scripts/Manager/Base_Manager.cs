using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance = null; // 싱글톤화

    #region Parameter

    private static Pool_Manager s_Pool = new Pool_Manager();
    private static Player_Manager s_Player = new Player_Manager();
    private static Data_Manager s_Data = new Data_Manager();
    private static Item_Manager s_Item = new Item_Manager();
    private static Inventory_Manager s_Inventory = new Inventory_Manager();
    private static Character_Manager s_Character = new Character_Manager();
    private static ADS_Manager s_ADS = new ADS_Manager();
    private static Firebase_Manager s_Firebase = new Firebase_Manager();
    public static Pool_Manager Pool { get { return s_Pool; } }
    public static Player_Manager Player { get { return s_Player; } }
    public static Data_Manager Data { get { return s_Data; } }
    public static Item_Manager Item { get { return s_Item; } }
    public static Inventory_Manager Inventory { get { return s_Inventory; } }
    public static Character_Manager Character { get { return s_Character; } }
    public static ADS_Manager ADS { get { return s_ADS; } }
    public static Firebase_Manager Firebase { get { return s_Firebase; } }


    #endregion

    public static bool isFast = false;

    float Save_Timer = 0.0f;

    private void Awake()
    {
        Initialize();
    }
    private void Update()
    {
        Save_Timer += Time.unscaledDeltaTime;
        if(Save_Timer >= 10.0f) // 10초마다 FireBase에 값 저장
        {
            Save_Timer = 0.0f;
            
            // Firebase가 초기화되었는지 확인
            if (Firebase != null && Firebase.reference != null)  // reference로 체크
            {
                Firebase.WriteData();
            }
            else
            {
                Debug.LogWarning("Firebase is not ready yet");
            }
        }

        for(int i = 0; i < Data.Buff_Timers.Length; i++)
        {
            if(Data.Buff_Timers[i] >= 0.0f)
            {
                Data.Buff_Timers[i] -= Time.unscaledDeltaTime;
            }
        }
        if(Data.Buff_x2 > 0.0f) Data.Buff_x2 -= Time.unscaledDeltaTime;
        
    }
    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;

            Pool.Initialize(transform);

            ADS.Init();
            Data.Init();
            Item.Init();
            Firebase.Init();

            Character.GetCharacter(0, "Hunter");

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    public void Coroutine_Action(float timer, Action action)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }

    IEnumerator Return_Pool_Coroutine(float time, GameObject obj, string path)
    {
        yield return new WaitForSeconds(time);
        Pool.m_pool_Dictionary[path].Return(obj);
    }

    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    private void OnDestroy() // 게임 종료시 데이터 저장용
    {
        // Firebase와 reference가 모두 null이 아닌지 확인
        if (Firebase != null && Firebase.reference != null)
        {
            Firebase.WriteData();
        }
        else
        {
            Debug.LogWarning("Firebase is not ready for final save");
        }
    }

}




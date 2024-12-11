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
    public static Pool_Manager Pool { get { return s_Pool; } }
    public static Player_Manager Player { get { return s_Player; } }
    public static Data_Manager Data { get { return s_Data; } }
    public static Item_Manager Item { get { return s_Item; } }


    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;

            Pool.Initialize(transform);
            Item.Init();    

            StartCoroutine(Action_Coroutine(() => Stage_Manager.State_Change(Stage_State.Ready), 0.3f));
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

}




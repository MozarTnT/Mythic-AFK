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
    public static Pool_Manager Pool { get { return s_Pool; } }
    public static Player_Manager Player { get { return s_Player; } }

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
            Stage_Manager.State_Change(Stage_State.Ready);

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




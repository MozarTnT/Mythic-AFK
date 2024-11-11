using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public interface IPool
{
    Transform parentTransform { get; set; } // 오브젝트 풀링 부모 폴더용

    Queue<GameObject> pool { get; set; } // 오브젝트 풀링 Queue

    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);
}
public class Object_Pool : IPool // 오브젝트 풀링 클래스
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public Transform parentTransform { get; set; }

    public GameObject Get(Action<GameObject> action = null) // Queue 에서 오브젝트를 가져오는 함수
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        if(action != null)
        {
            action?.Invoke(obj);
        }

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null) // Queue 에 오브젝트를 반환하는 함수
    {
        pool.Enqueue(obj);
        obj.transform.parent = parentTransform;
        //obj.transform.SetParent(parentTransform, false);

        obj.SetActive(false);

        if(action != null)
        {
            action?.Invoke(obj);
        }
    }


}

public class Pool_Manager // 오브젝트 풀링 매니저 클래스
{
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>(); // 오브젝트 풀링 딕셔너리
    Transform base_Obj = null;

    public void Initialize(Transform T)
    {
        base_Obj = T;
    }

    public IPool Pooling_OBJ(string path)
    {
        if(m_pool_Dictionary.ContainsKey(path) == false) // 키 값 확인 후 없으면 풀링 오브젝트 부모 폴더생성
        {
            Add_Pool(path);
        }

        if(m_pool_Dictionary[path].pool.Count <= 0) // 큐가 비어있다면 새 오브젝트 생성
        {
            Add_Queue(path);
        }

        return m_pool_Dictionary[path];
    }

    private GameObject Add_Pool(string path) // 풀링 오브젝트 생성 함수 (부모 폴더 생성)
    {
        GameObject obj = new GameObject("##POOL" + path);
        obj.transform.SetParent(base_Obj);
        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;

        return obj;
    }

    private void Add_Queue(string path) // 오브젝트 풀링 큐에 추가 (다 쓴 오브젝트 Queue로 이동)
    {
        var go = Base_Manager.instance.Instantiate_Path(path);
        go.transform.parent = m_pool_Dictionary[path].parentTransform;

        m_pool_Dictionary[path].Return(go);
    }

}

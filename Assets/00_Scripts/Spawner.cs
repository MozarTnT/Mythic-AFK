using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 스폰 기능 : 몬스터를 특정 위치에 특정 시간마다 랜덤으로 생성

    private int m_Count; // 생성 개수
    private float m_SpawnTime; // 몇 초마다 생성할지

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    Coroutine coroutine;

    private void Start()
    {
        Stage_Manager.m_ReadyEvent += OnReady;
        Stage_Manager.m_PlayEvent += OnPlay;
        Stage_Manager.m_BossEvent += OnBoss;
    }

    public void OnReady()
    {
        m_Count = int.Parse(CSV_Importer.Spawn_Design[Base_Manager.Data.Stage]["Spawn_Count"].ToString());
        m_SpawnTime = float.Parse(CSV_Importer.Spawn_Design[Base_Manager.Data.Stage]["Spawn_Timer"].ToString());
    }

    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }



    public void OnBoss()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        for(int i = 0; i < m_Monsters.Count; i++)
        {
            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(m_Monsters[i].gameObject);
        }
        m_Monsters.Clear();

        StartCoroutine(BossSetCoroutine());
    }


    IEnumerator BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        var monster = Instantiate(Resources.Load<GameObject>("Pool_OBJ/Boss"), Vector3.zero, Quaternion.Euler(0, 180, 0));
        monster.GetComponent<Monster>().Init();

        Vector3 pos = monster.transform.position;

        for(int i = 0; i < m_Players.Count; i++)
        {
            if(Vector3.Distance(pos, m_Players[i].transform.position) <= 3.0f)
            {
                m_Players[i].transform.LookAt(monster.transform.position);
                m_Players[i].KnockBack();
            }
        }

        yield return new WaitForSeconds(1.5f);

        m_Monsters.Add(monster.GetComponent<Monster>());

        Stage_Manager.State_Change(Stage_State.Boss_Play);
    }



    IEnumerator SpawnCoroutine() // 스폰 관련된 코루틴
    {
        Vector3 pos;
        //    10 =       35      -  25
        int value = m_Count - m_Monsters.Count;

        for(int i = 0; i < value; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            pos.y = 0f;

            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f) // 너무 가까운 경우에 다시 위치 재설정
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0f;
            }

            var goObj = Base_Manager.Pool.Pooling_OBJ("Monster").Get((value) =>
            {
                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                m_Monsters.Add(value.GetComponent<Monster>());
            });
        }
        
        yield return new WaitForSeconds(m_SpawnTime);

        coroutine = StartCoroutine(SpawnCoroutine());
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 스폰 조건 : 몬스터는 여러 마리가 특정 시간마다 수시로 스폰

    public GameObject monster_Prefab;

    public int m_Count; // 몬스터 숫자
    public float m_SpawnTime; // 몇 초마다 스폰할지

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void Update()
    {

    }
    IEnumerator SpawnCoroutine() // 몬스터 스폰용 코루틴
    {
        Vector3 pos;

        for(int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            pos.y = 0f;


            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f) // 너무 가까운 경우에 스폰 위치 재조정
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0f;
            }

            var go = Instantiate(monster_Prefab, pos, Quaternion.identity);
        }
        
        yield return new WaitForSeconds(m_SpawnTime);

        StartCoroutine(SpawnCoroutine());
    }
  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 스폰 기능 : 몬스터를 특정 위치에 특정 시간마다 랜덤으로 생성

    public GameObject monster_Prefab; // 몬스터 프리팹 오브젝트

    public int m_Count; // 생성 개수
    public float m_SpawnTime; // 몇 초마다 생성할지

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine() // 스폰 관련된 코루틴
    {
        Vector3 pos;

        for(int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            pos.y = 0f;

            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f) // 너무 가까운 경우에 다시 위치 재설정
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

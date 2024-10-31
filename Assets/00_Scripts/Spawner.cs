using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ���� ���� : ���ʹ� ���� ������ Ư�� �ð����� ���÷� ����

    public GameObject monster_Prefab;

    public int m_Count; // ���� ����
    public float m_SpawnTime; // �� �ʸ��� ��������

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void Update()
    {

    }
    IEnumerator SpawnCoroutine() // ���� ������ �ڷ�ƾ
    {
        Vector3 pos;

        for(int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            pos.y = 0f;


            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f) // �ʹ� ����� ��쿡 ���� ��ġ ������
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

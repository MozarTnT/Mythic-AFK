using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    RectTransform[] childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField] private float m_DistanceRange, speed;


    private void Awake()
    {
        cam = Camera.main;
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void Init(Vector3 pos)
    {
        // 초기 위치 설정
        target = pos;
        transform.position = cam.WorldToScreenPoint(pos); // 월드 좌표를 스크린 좌표로 변환

        // 자식 RectTransform 초기화
        for(int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero; // 자식 위치를 원점으로 설정
        }

        // 부모를 특정 레이어로 설정
        transform.parent = Base_Canvas.instance.HOLDER_LAYER(0);

        Base_Manager.Data.Money += Utils.Data.stageData.MONEY();

        // 코인 효과 코루틴 시작
        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect() // 코인 효과
    {
        // 랜덤 위치 배열 생성
        Vector2[] RandomPos = new Vector2[childs.Length];
        for(int i = 0; i < childs.Length; i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_DistanceRange, m_DistanceRange);
        }

        // 자식들이 랜덤 위치로 이동
        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed); // 위치 이동
            }

            // 모든 자식이 목표 위치에 도달했는지 확인
            if(Distance_Boolean(RandomPos, 0.5f))
            {
                break; // 루프 종료
            }

            yield return null; // 다음 프레임까지 대기
        }

        yield return new WaitForSeconds(0.3f); // 잠시 대기

        // 코인 목표 위치로 이동
        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.COIN.position, Time.deltaTime * (speed * 20.0f)); // 목표 위치로 이동
            }

            // 모든 자식이 목표 위치에 도달했는지 확인
            if(Distance_Boolean_World(0.5f))
            {
                Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject); // 풀에 오브젝트 반환
                break; // 루프 종료
            }
            yield return null; // 다음 프레임까지 대기
        }

        Main_UI.instance.TextCheck();
    }


    private bool Distance_Boolean(Vector2[] end, float range) // 모든 자식들의 위치와 목표 위치의 거리가 범위 이내인지 확인
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);

            if(distance > range)
            {
                return false;
            }
        }

        return true;
    }

    private bool Distance_Boolean_World(float range)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].position, Base_Canvas.instance.COIN.position);

            if(distance > range)
            {
                return false;
            }
        }

        return true;

    }
}

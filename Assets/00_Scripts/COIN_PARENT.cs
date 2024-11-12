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
        target = pos;

        transform.position = cam.WorldToScreenPoint(pos);

        for(int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }

        transform.parent = Base_Canvas.instance.HOLDER_LAYER(0);

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect() // 코인 효과
    {
        Vector2[] RandomPos = new Vector2[childs.Length];

        for(int i = 0; i < childs.Length; i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_DistanceRange, m_DistanceRange);
        }

        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed);
            }

            if(Distance_Boolean(RandomPos, 0.5f))
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.COIN.position, Time.deltaTime * (speed * 20.0f));
            }

            if(Distance_Boolean_World(0.5f))
            {
                Base_Manager.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }
            yield return null;
        }


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

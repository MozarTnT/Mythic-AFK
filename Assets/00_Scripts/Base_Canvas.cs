using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null; // 싱글턴 인스턴스

    private void Awake()
    {
        // 싱글턴 패턴 구현
        if(instance == null)
        {
            instance = this; // 인스턴스 설정
            DontDestroyOnLoad(this.gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(this.gameObject); // 이미 존재하는 인스턴스가 있으면 현재 오브젝트 파괴
        }
    }
   
    public Transform COIN; // 코인 오브젝트
    [SerializeField] private Transform LAYER; // 레이어 설정

    // 주어진 인덱스에 해당하는 LAYER의 자식 Transform 반환
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }
}

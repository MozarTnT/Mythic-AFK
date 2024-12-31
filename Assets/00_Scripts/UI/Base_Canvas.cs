using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        HERO_BUTTON.onClick.AddListener(() => GetUI("#Heros", true));
        INVENTORY_BUTTON.onClick.AddListener(() => GetUI("#Inventory"));
    }
   
    public Transform COIN; // 코인 오브젝트
    [SerializeField] private Transform LAYER; // 레이어 설정
    [SerializeField] private Button HERO_BUTTON, INVENTORY_BUTTON;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Utils.UI_Holder.Count > 0)
                Utils.ClosePopupUI();
            else
            {
                Debug.Log("게임 종료");
            }
        }
    }


    // 주어진 인덱스에 해당하는 LAYER의 자식 Transform 반환
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

    public void GetUI(string temp, bool Fade = false)
    {
        if(Fade)
        {
            Main_UI.instance.FadeInOut(false, true,() => GetPopupUI(temp));
            return;
        }
        GetPopupUI(temp);
    }

    void GetPopupUI(string temp)
    {
        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), transform);
        Utils.UI_Holder.Push(go);
    }
}

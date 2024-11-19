using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HIT_TEXT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    public TextMeshProUGUI m_Text;

    [SerializeField] private GameObject m_Critical;

    float UpRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double dmg, bool Monster = false, bool Critical = false)
    {
        // 타겟 위치를 랜덤하게 조정
        pos.x += Random.Range(-0.1f, 0.1f);
        pos.z += Random.Range(-0.1f, 0.1f);

        target = pos; // 타겟 위치 설정
        m_Text.text = StringMethod.ToCurrencyString(dmg); // 데미지 텍스트 설정

        if(Monster)
        {
            m_Text.color = Color.red;
        }
        else
        {
            m_Text.color = Color.white;
        }


        transform.parent = Base_Canvas.instance.HOLDER_LAYER(1); // UI 캔버스의 특정 레이어에 자식으로 설정

        m_Critical.SetActive(Critical); // 크리티컬 여부에 따라 활성화

        // 풀에 오브젝트 반환
        Base_Manager.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT");
    }

    private void Update()
    {
        // 타겟 위치에 따라 오브젝트 위치 업데이트
        Vector3 targetPos = new Vector3(target.x, target.y + UpRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos); // 월드 좌표를 스크린 좌표로 변환

        // UpRange가 0.3f 이하일 때 증가
        if(UpRange <= 0.3f)
        {
            UpRange += Time.deltaTime; // UpRange 증가
        }
    }

    private void ReturnText()
    {
        Base_Manager.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }
  
}

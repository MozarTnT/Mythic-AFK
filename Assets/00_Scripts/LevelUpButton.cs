using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // UI 요소들 참조
    [SerializeField] private Image m_EXP_Slider;        // 경험치 게이지 이미지
    [SerializeField] private TextMeshProUGUI EXP_Text, ATK_Text, GoldText, HP_Text, Get_EXP_Text;    // 각종 상태 텍스트

    bool isPush = false;    // 버튼 누르고 있는지 체크
    float timer = 0f;       // 연속 클릭 타이머
    Coroutine coroutine;    // 코루틴 참조 저장용

    void Start()
    {
        InitEXP();    // 초기 경험치 UI 설정
    }

    void Update()
    {
        // 버튼을 누르고 있을 때 연속 실행
        if (isPush)
        {
            timer += Time.deltaTime;
            if (timer >= 0.01f)    // 0.01초마다 실행
            {
                timer = 0f;
                EXP_UP();          // 경험치 증가 함수 호출
            }
        }
    }

    // 경험치 증가 및 UI 업데이트
    public void EXP_UP()
    {
        Base_Manager.Player.EXP_UP();    // 플레이어 경험치 증가
        InitEXP();                       // UI 업데이트
        transform.DORewind();            // DOTween 애니메이션 초기화
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);    // 버튼 펀치 애니메이션
    }

    // 버튼 클릭 시 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        EXP_UP();    // 즉시 경험치 증가
        coroutine = StartCoroutine(Push_Coroutine());    // 연속 클릭 코루틴 시작
    }

    // 버튼에서 손을 뗄 때 호출
    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;    // 연속 클릭 중지
        if (coroutine != null)
        {
            StopCoroutine(coroutine);    // 코루틴 중지
        }
        timer = 0f;    // 타이머 초기화
    }

    // UI 텍스트 업데이트
    private void InitEXP()
    {
        m_EXP_Slider.fillAmount = Base_Manager.Player.EXP_Percentage();    // 경험치 게이지 업데이트
        EXP_Text.text = string.Format("{0:0.00}%", Base_Manager.Player.EXP_Percentage() * 100.0f);    // 경험치 퍼센트 표시
        ATK_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.ATK());          // 다음 공격력 증가량
        HP_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.HP());            // 다음 체력 증가량
        Get_EXP_Text.text = "<color=#00FF00>EXP</color> +" + string.Format("{0:0.00}", Base_Manager.Player.Next_EXP()) + "%";    // 획득 경험치량
    }

    // 1초 후에 연속 클릭 시작
    IEnumerator Push_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);    // 1초 대기
        isPush = true;    // 연속 클릭 활성화
    }
}

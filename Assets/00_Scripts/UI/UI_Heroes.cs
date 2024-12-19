using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes : UI_Base
{
    // UI 요소들
    public Transform Content; // 캐릭터 UI를 배치할 부모 Transform
    public GameObject Part;   // 캐릭터 UI 파트의 프리팹
    public List<UI_Heroes_Part> parts = new List<UI_Heroes_Part>();

    // 캐릭터 데이터 저장을 위한 딕셔너리
    Dictionary<string, Character_Scriptable> m_Dictionries = new Dictionary<string, Character_Scriptable>();
    Character_Scriptable m_Character;
    

    public void InitButtons()
    {
        for(int i = 0; i < Render_Manager.instance.HERO.Circles.Length; i++)
        {
            int index = i;
            var go = new GameObject("Button").AddComponent<Button>(); // 버튼 생성 
            go.onClick.AddListener(() => Set_Character_Button(index));

            go.transform.SetParent(this.transform); // UI Heroes 하단에 생성
            go.gameObject.AddComponent<Image>(); // 이미지 컴포넌트 추가

            //go.gameObject.AddComponent<RectTransform>(); // 좌표용 RectTransform 추가

            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero; // 좌표 초기화 (앵커 프리셋용) -> 해상도 변경시에도 같은 좌표 유지 위해서
            rect.offsetMax = Vector2.zero; 

            rect.sizeDelta = new Vector2(150.0f, 150.0f); // 버튼 크기 설정
            rect.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.01f); // 투명하게 설정

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);

        }
    }

    private void Set_Character_Button(int value)
    {
        Base_Manager.Character.GetCharacter(value, m_Character.m_Character_Name);

        Render_Manager.instance.HERO.GetParticle(false);
        Set_Click(null);
        Render_Manager.instance.HERO.InitHero();

        for(int i = 0; i < parts.Count; i++)
        {
            parts[i].Get_Character_Check();
        }

        Main_UI.instance.Set_Character_Data();
    }


    public void Set_Click(UI_Heroes_Part s_Part)
    {
        if(s_Part == null)
        {
            for(int i = 0; i < parts.Count; i++)
            {
                parts[i].LockOBJ.SetActive(false);
                parts[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            m_Character = s_Part.m_Character;
            for(int i = 0; i < parts.Count; i++)
            {
                parts[i].LockOBJ.SetActive(true);
                parts[i].GetComponent<Outline>().enabled = false;
            }
            s_Part.LockOBJ.SetActive(false);
            s_Part.GetComponent<Outline>().enabled = true;
        }

    }

    public override bool Init()
    {
        InitButtons();

        Render_Manager.instance.HERO.InitHero();

        Main_UI.instance.FadeInOut(true, true, null);

        // Resources 폴더에서 모든 캐릭터 데이터 로드
        var Datas = Base_Manager.Data.m_Data_Character;
        
        // 로드한 데이터로 딕셔너리 초기화
        foreach(var data in Datas)
        {
            m_Dictionries.Add(data.Value.Data.m_Character_Name, data.Value.Data); // 캐릭터 이름을 키로 사용
        }

        // 딕셔너리를 Rarity에 따라 정렬
        var sort_Dictionary = m_Dictionries.OrderByDescending(x => x.Value.m_Rarity);

        // 정렬된 캐릭터 데이터를 UI에 표시
        foreach(var data in sort_Dictionary)
        {
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            parts.Add(go);
            go.Initialize(data.Value, this); 
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.FadeInOut(false, true, () =>
        {
            Main_UI.instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });
    }


}

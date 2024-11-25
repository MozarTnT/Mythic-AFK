using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Heroes : UI_Base
{
    // UI 요소들
    public Transform Content; // 캐릭터 UI를 배치할 부모 Transform
    public GameObject Part;   // 캐릭터 UI 파트의 프리팹

    // 캐릭터 데이터 저장을 위한 딕셔너리
    Dictionary<string, Character_Scriptable> m_Dictionries = new Dictionary<string, Character_Scriptable>();

    public override bool Init()
    {
        Main_UI.instance.FadeInOut(true, true, null);

        // Resources 폴더에서 모든 캐릭터 데이터 로드
        var Data = Resources.LoadAll<Character_Scriptable>("Scriptable");
        
        // 로드한 데이터로 딕셔너리 초기화
        for(int i = 0; i < Data.Length; i++)
        {
            m_Dictionries.Add(Data[i].m_CharacterName, Data[i]); // 캐릭터 이름을 키로 사용
        }

        // 딕셔너리를 Rarity에 따라 정렬
        var sort_Dictionary = m_Dictionries.OrderByDescending(x => x.Value.m_Rarity);

        // 정렬된 캐릭터 데이터를 UI에 표시
        foreach(var data in sort_Dictionary)
        {
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            go.Initialize(data.Value); 
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

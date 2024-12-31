using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    public enum InventoryState { ALL, EQUIPMENT, CONSUMABLE, OTHER }
    [SerializeField] InventoryState m_InventoryState;
    [SerializeField] RectTransform m_Bar;
    [SerializeField] private Button[] m_Top_Buttons;
    [SerializeField] RectTransform TopContent;

    [SerializeField] private Transform Content;
    public UI_Inventory_Part part;
    public override bool Init()
    {
        var sort_Dictionary = Base_Manager.Inventory.m_Items.OrderByDescending(x => x.Value.data.rarity);

        foreach(var item in sort_Dictionary)
        {
            Instantiate(part, Content).Init(item.Value);  
        }
        
        for(int i = 0; i < m_Top_Buttons.Length; i++)
        {
            int index = i;
            m_Top_Buttons[index].onClick.AddListener(() => Item_Inventory_Check((InventoryState)index));
        }

        return base.Init();
    }

    public void Item_Inventory_Check(InventoryState m_State)
    {
        m_InventoryState = m_State;
        StartCoroutine(barMovementCoroutine(
            m_Top_Buttons[(int)m_State].GetComponent<RectTransform>().anchoredPosition,
            m_Top_Buttons[(int)m_State].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x));
    }

    IEnumerator barMovementCoroutine(Vector2 endPos, float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, TopContent.anchoredPosition.y);

        float startX = m_Bar.sizeDelta.x;
        float endX = endXPos + 60.0f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            float LerpPosX = Mathf.Lerp(startX, endX, percent);

            m_Bar.anchoredPosition = LerpPos;
            // 늘어나는 최소값 지정
            m_Bar.sizeDelta = new Vector2(Mathf.Clamp(LerpPosX, 200.0f, Mathf.Infinity),m_Bar.sizeDelta.y);

            yield return null;
        }

    }
    
}

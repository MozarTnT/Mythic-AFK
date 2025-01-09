using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SavingMode : UI_Base
{
    [SerializeField] private TextMeshProUGUI BatteryText, TimerText, FightText, StageText;
    [SerializeField] private Image BatteryFill;
    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part item_Part;

    public Dictionary<string, Item> m_SaveItem = new Dictionary<string, Item>();

    private void Update()
    {
        // BatteryLevel 0.0f ~ 1.0f
        BatteryText.text = (SystemInfo.batteryLevel * 100).ToString() + "%";
        BatteryFill.fillAmount = SystemInfo.batteryLevel;

        // 시간
        TimerText.text = System.DateTime.Now.ToString("tt hh:mm");

        int stageValue = Base_Manager.Data.Stage + 1;
        int stageForward = (stageValue / 10) + 1;
        int stageBack = stageValue % 10;

        StageText.text = "보통 " + stageForward.ToString() + " - " + stageBack.ToString();
        FightText.text = Stage_Manager.isDead ? "반복중..." : "진행중...";

    }

    public void GetItem(Item_Scriptable item)
    {
        if(m_SaveItem.ContainsKey(item.name))
        {
            m_SaveItem[item.name].Count++;
            return;
        }

        Item items = new Item{data = item, Count = 1};

        m_SaveItem.Add(item.name, items);
        var go = Instantiate(item_Part, Content);
        go.Init(items);
        
    }

}

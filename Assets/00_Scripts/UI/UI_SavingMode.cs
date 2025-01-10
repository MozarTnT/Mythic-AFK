using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnSavingMode();

public class UI_SavingMode : UI_Base
{
    [SerializeField] private TextMeshProUGUI BatteryText, TimerText, FightText, StageText;
    [SerializeField] private Image BatteryFill, LandImage;
    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part item_Part;

    public Dictionary<string, Item> m_SaveItem = new Dictionary<string, Item>();
    public Dictionary<string, UI_Inventory_Part> m_Parts = new Dictionary<string, UI_Inventory_Part>();

    public static OnSavingMode m_OnSaving;

    Vector2 StartPos, EndPos;
    Camera camera;

    public override bool Init()
    {
        camera = Camera.main;
        camera.enabled = false;

        m_OnSaving?.Invoke();

        return base.Init();
    }

    public override void DisableOBJ()
    {
        camera.enabled = true;
        Base_Canvas.isSave = false;
        base.DisableOBJ();
    }

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

        if(Input.GetMouseButtonDown(0)) // 마우스 클릭시
        {
            StartPos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0)) // 마우스를 누르는 동안
        {
            EndPos = Input.mousePosition;

            float distance = Vector2.Distance(EndPos, StartPos);
            LandImage.color = new Color(1, 1, 1, Mathf.Clamp(distance / (Screen.width / 2), 0.3f, 1.0f));

            if(distance >= Screen.width / 2)
            {
                DisableOBJ();
            }
        }

        if(Input.GetMouseButtonUp(0)) // 마우스가 떨어지면
        {
            StartPos = Vector2.zero;
            EndPos = Vector2.zero;
            LandImage.color = new Color(1, 1, 1, 0.3f);
        }


    }

    public void GetItem(Item_Scriptable item)
    {
        if(m_SaveItem.ContainsKey(item.name))
        {
            m_SaveItem[item.name].Count++;
            m_Parts[item.name].Init(m_SaveItem[item.name]);
            return;
        }

        Item items = new Item{data = item, Count = 1};

        m_SaveItem.Add(item.name, items);
        var go = Instantiate(item_Part, Content);
        m_Parts.Add(item.name, go);
        go.Init(items);
    }

}

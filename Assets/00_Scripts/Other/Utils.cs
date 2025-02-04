using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static LevelDesign Data = Resources.Load<LevelDesign>("Scriptable/LevelDesignData");
    public static void CloseAllPopupUI()
    {
        while(UI_Holder.Count > 0) ClosePopupUI();
    }

    public static void ClosePopupUI()
    {
        if(UI_Holder.Count== 0) return;

        UI_Base popup = UI_Holder.Peek();
        popup.DisableOBJ();
    }

    public static Sprite Get_Atlas(string temp)
    {
        return m_Atlas.GetSprite(temp);
    }
    public static string String_Color_Rarity(Rarity rare)
    {
        switch(rare)
        {
            case Rarity.Common: return "<color=#FFFFFF>";
            case Rarity.Uncommon: return "<color=#00FF00>";
            case Rarity.Rare: return "<color=#00D8FF>";
            case Rarity.Hero: return "<color=#B750C3>";
            case Rarity.Legendary: return "<color=#FF9000>"; 
        }

        return "<color=#FFFFFF>";
    }

    public static string GetTimer(float time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);

        return timer;
    }

    // 지수 증가 공식 -> 값을 일정 비율로 지속적 증가시킴
    public static double CalculateValue(float baseValue, int Level, float value)
    {
        return baseValue * Mathf.Pow((Level + 1), value);
    }

    public static bool Coin_Check(double value)
    {
        if(Data_Manager.m_Data.Money >= value)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    
}

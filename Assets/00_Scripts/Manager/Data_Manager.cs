using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//서버 관리용 DB 데이터 저장 용도


public class Character_Holder
{
    public Character_Scriptable Data;
    public int Level;
    public int Count;
}

public class Data_Manager
{
    public double Money;
    public int Level;
    public double EXP;
    public int Stage;
    public float[] Buff_Timers = {0.0f, 0.0f, 0.0f};
    public float Buff_x2 = 0.0f;
    public int BuffLevel, BuffCount;
    public Dictionary<string, Character_Holder> m_Data_Character = new Dictionary<string, Character_Holder>(); // 플레이어가 가지고 있는 캐릭터 저장용

    public void Init()
    {
        Set_Character();
    }

    private void Set_Character()
    {
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var data in datas)
        {
            var character = new Character_Holder();

            character.Data = data;
            character.Level = 0;
            character.Count = 0;

            m_Data_Character.Add(data.m_Character_Name, character);
        }
    }
    
}

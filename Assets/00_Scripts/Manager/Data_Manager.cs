using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//서버 관리용 DB 데이터 저장 용도


public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;
}


public class Holder
{
    public int Level;
    public int Count;
}

public class Data
{
    public double Money;
    public int Level;
    public double EXP;
    public int Stage;
    public float[] Buff_Timers = {0.0f, 0.0f, 0.0f};
    public float Buff_x2 = 0.0f;
    public int BuffLevel, BuffCount;

}

public class Data_Manager
{
    public static Data m_Data = new Data();
    public Dictionary<string, Holder> Character_Holder = new Dictionary<string, Holder>();
   
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
            Holder s_holder = new Holder();

            if(Character_Holder.ContainsKey(data.m_Character_Name))
            {
                s_holder = Character_Holder[data.m_Character_Name];
                Debug.Log(data.m_Character_Name + " : " + s_holder.Level + " : " + s_holder.Count);
            }
            else
            {
                Character_Holder.Add(data.m_Character_Name, s_holder);
            }

            character.holder = s_holder;

            m_Data_Character.Add(data.m_Character_Name, character);
        }
    }
    
}

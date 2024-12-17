using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager
{
    public Character_Holder[] m_Set_Character = new Character_Holder[6];

    public void GetCharacter(int value, string character_Name)
    {
        m_Set_Character[value] = Base_Manager.Data.m_Data_Character[character_Name];
    }

}

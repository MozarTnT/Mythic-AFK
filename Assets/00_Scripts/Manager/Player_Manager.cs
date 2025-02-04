using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager
{
    public double ATK = 10;
    public double HP = 50;
    public float Critical_Percentage = 20.0f;
    public double Critical_Damage = 140.0d;


    public void EXP_UP()
    {
        Data_Manager.m_Data.EXP += Utils.Data.levelData.EXP();
        ATK += Utils.Data.levelData.ATK();
        HP += Utils.Data.levelData.HP();
        if(Data_Manager.m_Data.EXP >= Utils.Data.levelData.MAXEXP())
        {

            Data_Manager.m_Data.Level++;
            Data_Manager.m_Data.EXP = 0;
            Main_UI.instance.TextCheck();

        }

        for(int i = 0; i < Spawner.m_Players.Count; i++)
        {
            Spawner.m_Players[i].Set_ATKHP();
        }
    }

    public float EXP_Percentage()
    {
        float exp = (float)Utils.Data.levelData.MAXEXP();
        double myEXP = Data_Manager.m_Data.EXP;


        return (float)myEXP / exp;
    }

    public float Next_EXP()
    {
        float exp = (float)Utils.Data.levelData.MAXEXP();
        float myExp = (float)Utils.Data.levelData.EXP();

        return (myExp / exp) * 100.0f;
    }


    public double Get_ATK(Rarity rarity) // 나중에 레벨디자인
    {
        return ATK * ((int)rarity + 1);
    }

    public double Get_HP(Rarity rarity)
    {
        return HP * ((int)rarity + 1);
    }

    public double Average_ATK() // 평균 공격력 계산식
    {
        return ATK + HP;
    }
}

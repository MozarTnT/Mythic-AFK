using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager
{
    public int Level;
    public double EXP;
    public double ATK = 10;
    public double HP = 50;
    public float Critical_Percentage = 20.0f;
    public double Critical_Damage = 140.0d;


    public void EXP_UP()
    {
        EXP += float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());
        ATK += Next_ATK();
        HP += Next_HP();
        if(EXP >= float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString()))
        {
            Level++;
            Main_UI.instance.TextCheck();
        }

        for(int i = 0; i < Spawner.m_Players.Count; i++)
        {
            Spawner.m_Players[i].Set_ATKHP();
        }
    }

    public float EXP_Percentage()
    {
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        double myEXP = EXP;

        if(Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
            myEXP -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }

        return (float)myEXP / exp;
    }

    public float Next_EXP()
    {
        float exp = float.Parse(CSV_Importer.EXP[Level]["EXP"].ToString());
        float myExp = float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString());
        if(Level >= 1)
        {
            exp -= float.Parse(CSV_Importer.EXP[Level - 1]["EXP"].ToString());
        }

        return (myExp / exp) * 100.0f;
    }

    public double Next_ATK()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 5;
    }

    public double Next_HP()
    {
        return float.Parse(CSV_Importer.EXP[Level]["Get_EXP"].ToString()) * (Level + 1) / 3;
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

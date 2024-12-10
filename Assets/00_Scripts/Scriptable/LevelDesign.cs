using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public int currentLevel;
    public int currentStage;

    public LevelData levelData;
    [Space(20.0f)]
    public StageData stageData;
}

[System.Serializable]
public class LevelData
{
    public int currentLevel;
    [Range(0.0f, 10.0f)]
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY;

    [Space(20.0f)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_EXP;
    public int B_MAXEXP;
    public int B_MONEY;

    public double ATK() => Utils.CalculateValue(B_ATK, Base_Manager.Data.Level, C_ATK);
    public double HP() => Utils.CalculateValue(B_HP, Base_Manager.Data.Level, C_HP);
    public double EXP() => Utils.CalculateValue(B_EXP, Base_Manager.Data.Level, C_EXP);
    public double MAXEXP() => Utils.CalculateValue(B_MAXEXP, Base_Manager.Data.Level, C_MAXEXP);
    public double MONEY() => Utils.CalculateValue(B_MONEY, Base_Manager.Data.Level, C_MONEY);
}

[System.Serializable]
public class StageData
{
    public int currentStage;
    [Range(0.0f, 10.0f)]
    public float M_ATK, M_HP, M_MONEY;

    [Space(20.0f)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_MONEY;

    public double ATK() => Utils.CalculateValue(B_ATK, Base_Manager.Data.Stage, M_ATK);
    public double HP() => Utils.CalculateValue(B_HP, Base_Manager.Data.Stage, M_HP);
    public double MONEY() => Utils.CalculateValue(B_MONEY, Base_Manager.Data.Stage, M_MONEY);
}

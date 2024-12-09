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
    public float B_ATK;
    public float B_HP;
    public float B_EXP;
    public float B_MAXEXP;
    public float B_MONEY;
}

[System.Serializable]
public class StageData
{
    public int currentStage;
    [Range(0.0f, 10.0f)]
    public float m_ATK, m_HP, m_MONEY;

    [Space(20.0f)]
    [Header("## Base Value")]
    public float B_ATK;
    public float B_HP;
    public float B_MONEY;
}

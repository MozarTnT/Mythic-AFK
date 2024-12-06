using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public LevelData levelData;

    // 지수 증가 공식 -> 값을 일정 비율로 지속적 증가시킴
    public float CalculateValue(float baseValue, int Level, float value)
    {
        return baseValue * Mathf.Pow(Level, value);
    }
}

[System.Serializable]
public class LevelData
{
    public int currentLevel;
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY;
}

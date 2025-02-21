using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public GameObject SkillParticle;

    protected Monster[] monsters {get {return Spawner.m_Monsters.ToArray();}}
    protected Player[] players {get {return Spawner.m_Players.ToArray();}}
    protected Character m_Player {get {return GetComponent<Character>();}}
    private void Start()
    {
        Stage_Manager.m_DeadEvent += OnDead;
    }
    public virtual void Set_Skill()
    {
        
    }

    protected double Skill_Damage(double value)
    {
        return m_Player.ATK * (value / 100.0f);
    }

    protected bool Distance(Vector3 startPos, Vector3 endPos, float distanceValue)
    {
        return Vector3.Distance(startPos, endPos) <= distanceValue;
    }


    private void OnDead()
    {
        StopAllCoroutines();
    }

    public virtual void ReturnSkill() // 스킬 종료 호출 접근은 public
    {
        m_Player.isGetSkill = false;
        m_Player.AnimatorChange("isIDLE");
    }

    public Character HP_Check() // 가장 낮은 체력 캐릭터 추적
    {
        Character player = null;
        double hpCount = Mathf.Infinity;

        for(int i = 0; i < players.Length; i ++)
        {
            double hp = players[i].HP;

            if(hp < hpCount)
            {
                hpCount = hp;
                player = players[i];
            }

        }
        return player;

    }

}

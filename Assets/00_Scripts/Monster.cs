using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Character
{
    public float m_Speed;
    bool isSpawn = false;

    public double R_ATK, R_HP;
    public float R_Attack_Range;
    public bool isBoss = false;

    protected override void Start()
    {
        base.Start();
    }

    public void Init()
    {
        isDead = false;
        ATK = R_ATK;
        HP = R_HP;
        Attack_Range = R_Attack_Range;
        Target_Range = Mathf.Infinity;

        if(isBoss)
        {
            StartCoroutine(SkillCoroutine());
        }

        StartCoroutine(Spawn_Start());
    }

    IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Skill_Base>().Set_Skill();

        StartCoroutine(SkillCoroutine());
    }

    private void Update()
    {
        if (isSpawn == false) return;
        if(Stage_Manager.m_State == Stage_State.Play || Stage_Manager.m_State == Stage_State.Boss_Play)
        {
            if(m_Target == null) FindClosestTarget(Spawner.m_Players.ToArray());

            // 타겟과의 거리 계산
            if(m_Target != null)
            {
                float targetDistance = Vector3.Distance(transform.position, m_Target.position);

                if(targetDistance > Attack_Range && isAttack == false)
                {
                    // 추적 범위 내에 있을 때 이동
                    AnimatorChange("isMOVE");
                    transform.LookAt(m_Target.position);
                    transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
                }
                else if(targetDistance <= Attack_Range && isAttack == false)
                {
                    // 공격 범위 내에 있을 때 공격
                    isAttack = true;
                    AnimatorChange("isATTACK");
                    Invoke("InitAttack", 1.0f);
                }
            }
        }
      
    }

    IEnumerator Spawn_Start()
    {
        float current = 0f;
        float percent = 0f;
        float start = 0f;
        float end = transform.localScale.x;

        while(percent < 1)
        {
            current += Time.deltaTime; // 시간 증가
            percent = current / 0.2f;
                           // 애니메이션 변환 (시작, 끝, 시간) => 애니메이션 변환 중 중간 위치 계산
            float LerpPos = Mathf.Lerp(start, end, percent); 
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    public override void GetDamage(double dmg)
    {
        if(isDead) return;

        bool critical = Critical(ref dmg);

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, false, critical);
        });

        HP -= dmg;

        if(isBoss)
        {
            Main_UI.instance.Boss_Slider_Count(HP, 500);
        }

        if(HP <= 0)
        {
            isDead = true;
            Dead_Event();
        }
    }

    private void Dead_Event()
    {
        if(!isBoss)
        {
            Stage_Manager.Count++;
            Main_UI.instance.Monster_Slider_Count();
        }
        else
        {
            Stage_Manager.State_Change(Stage_State.Clear);
        }

        Spawner.m_Monsters.Remove(this);

        Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().main.duration, value, "Smoke");
        });

        Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<COIN_PARENT>().Init(transform.position);
        });


        for(int i = 0; i < 3; i++) // 아이템 테스트용
        {
                Base_Manager.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
                {
                    value.GetComponent<Item_OBJ>().Init(transform.position);
            });
        }

        if(!isBoss)
        {
            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private bool Critical(ref double dmg)
    {
        float RandomValue = Random.Range(0, 100.0f);

        if(RandomValue <= Base_Manager.Player.Critical_Percentage)
        {
            dmg *= (Base_Manager.Player.Critical_Damage / 100);
            return true;
        }
        return false;
        
    }

}

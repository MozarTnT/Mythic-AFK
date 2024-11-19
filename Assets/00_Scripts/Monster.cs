using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Character
{
    public float m_Speed;
    bool isSpawn = false;



    protected override void Start()
    {
        base.Start();
        HP = 500;
    }

    public void Init()
    {
        isDead = false;
        HP = 500;
        Attack_Range = 0.5f;
        StartCoroutine(Spawn_Start());
    }

    private void Update()
    {
        if (isSpawn == false) return;

        FindClosestTarget(Spawner.m_Players.ToArray());

        if(m_Target.GetComponent<Character>().isDead) FindClosestTarget(Spawner.m_Players.ToArray());
        
        // 타겟과의 거리 계산
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

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, false);
        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
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

            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
    }

}

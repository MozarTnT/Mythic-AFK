using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEditor.SceneManagement;

public class Player : Character
{
    public Character_Scriptable CH_Data;
    public ParticleSystem Provocation_Effect;
    public GameObject TrailObject;
    public string CH_Name;
    public int MP;
    Vector3 startPos;
    Quaternion rot;
    public bool MainCharacter = false;

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/Character/" + CH_Name));

        Spawner.m_Players.Add(this);

        Stage_Manager.m_ReadyEvent += OnReady;
        Stage_Manager.m_BossEvent += OnBoss;
        Stage_Manager.m_ClearEvent += OnClear;
        Stage_Manager.m_DeadEvent += OnDead;

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Bullet_Name = CH_Data.m_Character_Name;
        Attack_Range = data.m_Attack_Range;

        Set_ATKHP();
    }

    public void Set_ATKHP()
    {
        ATK = Base_Manager.Player.Get_ATK(CH_Data.m_Rarity);
        HP = Base_Manager.Player.Get_HP(CH_Data.m_Rarity);
    }


    private void OnReady()
    {
        AnimatorChange("isIDLE");
        isDead = false;
        Spawner.m_Players.Add(this);
        Set_ATKHP();
        transform.position = startPos;
        transform.rotation = rot;
    }

    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }

    private void OnClear()
    {
        AnimatorChange("isCLEAR");
    }

    private void OnDead()
    {
        Spawner.m_Players.Add(this);
    }

    public void Get_MP(int mp)
    {
        if(MainCharacter) return;

        Main_UI.instance.Character_State_Check(this);
        MP += mp;

    }

    private void Update()
    {
        if(isDead) return;

        if(Stage_Manager.m_State == Stage_State.Play || Stage_Manager.m_State == Stage_State.Boss_Play)
        {
            FindClosestTarget(Spawner.m_Monsters.ToArray());

            // 타겟이 없을 경우 가장 가까운 타겟을 찾음
            if(m_Target == null)
            {      
                // 시작 위치로 돌아가는 로직
                float targetPos = Vector3.Distance(transform.position, startPos);
                if(targetPos >= 0.1f)
                {
                    // 이동 및 회전
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                    transform.LookAt(startPos);
                    AnimatorChange("isMOVE");
                }
                else
                {
                    // 제자리에서 대기
                    transform.rotation = rot;
                    AnimatorChange("isIDLE");
                }
            }
            else
            {
                if(m_Target.GetComponent<Character>().isDead)
                {
                    FindClosestTarget(Spawner.m_Monsters.ToArray());
                }
         
                // 타겟과의 거리 계산
                float targetDistance = Vector3.Distance(transform.position, m_Target.position);
                if(targetDistance <= Target_Range && targetDistance > Attack_Range && isAttack == false)
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
                    Get_MP(5);
                    Invoke("InitAttack", 1.0f);
                }
            }
        }
    }

    public void KnockBack()
    {
        StartCoroutine(KnockBackCoroutine(3.0f, 0.3f));
    }

    IEnumerator KnockBackCoroutine(float power, float duration)
    {
        float t = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0f;

        while(t > 0f)
        {
            t -= Time.deltaTime;
            if(Vector3.Distance(Vector3.zero, transform.position) < 3.0f)
            {
                transform.position += force * Time.deltaTime;
            }
            yield return null;
        }
    }

    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        if(Stage_Manager.isDead) return;

        Get_MP(3);


        var goObj = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, true);
        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            DeadEvent();
        }
    }

    private void DeadEvent()
    {
        Spawner.m_Players.Remove(this);
        if(Spawner.m_Players.Count <= 0 && Stage_Manager.isDead == false)
        {
            Stage_Manager.State_Change(Stage_State.Dead);
        }
        AnimatorChange("isDEAD");
        m_Target = null;
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObject.SetActive(true);

        Invoke("TrailDisable", 1.0f);
    }

    private void TrailDisable() => TrailObject.SetActive(false);

}

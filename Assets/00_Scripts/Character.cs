using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;      
    public double HP;
    public double ATK;
    public float ATK_Speed;

    public bool isDead = false;

    protected float Attack_Range = 3.0f;
    protected float Target_Range = 5.0f;
    protected bool isAttack = false;
    protected Transform m_Target;

    [SerializeField] private Transform m_BulletTransform;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isAttack = false;

    protected void AnimatorChange(string temp) // 애니메이션 변환
    {
        if(temp == "isATTACK")
        {
            animator.SetTrigger("isATTACK");
            return;
        }

        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

  

    protected virtual void Bullet()
    {
        if(m_Target == null) return;
        
        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.transform.LookAt(m_Target);
            value.GetComponent<Bullet>().Init(m_Target, ATK, "CH_01");
        });
    }

    protected virtual void Attack()
    {
        if(m_Target == null) return;

        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_Target.position;
            value.GetComponent<Bullet>().Attack_Init(m_Target, ATK);
        });


    }

    public virtual void GetDamage(double dmg)
    {
        
    }


    protected void FindClosestTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closestTarget = null;
        float maxDistance = Target_Range;

        foreach(var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }
        m_Target = closestTarget;
        if(m_Target != null)
        {
            transform.LookAt(m_Target.position);
        }

    }
}

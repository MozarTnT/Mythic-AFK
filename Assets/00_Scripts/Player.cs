using UnityEngine;
using System.Linq;
using System.Collections;

public class Player : Character
{
    Vector3 startPos;
    Quaternion rot;

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Update()
    {
        if(m_Target == null)
        {
            FindClosestTarget(Spawner.m_Monsters.ToArray());

            float targetPos = Vector3.Distance(transform.position, startPos);
            if(targetPos >= 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange("isMOVE");
            }
            else
            {
                transform.rotation = rot;
                AnimatorChange("isIDLE");
            }
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        if(targetDistance <= Target_Range && targetDistance > Attack_Range && isAttack == false) // 추적 범위 안에 있지만 공격 범위가 안될때
        {
            AnimatorChange("isMOVE");
            transform.LookAt(m_Target.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
        }
        else if(targetDistance <= Attack_Range && isAttack == false) // 공격 범위 안에 있을때
        {
            isAttack = true;
            AnimatorChange("isATTACK");
            Invoke("InitAttack", 1.0f);
        }

    }

}

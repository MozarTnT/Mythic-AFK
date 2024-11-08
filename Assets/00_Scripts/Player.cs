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
        FindClosestTarget(Spawner.m_Monsters.ToArray());

        // 타겟이 없을 경우 가장 가까운 타겟을 찾음
        if(m_Target == null)
        {
            FindClosestTarget(Spawner.m_Monsters.Select(m => m.transform).ToArray());

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
            
            return;
        }

        if(m_Target.GetComponent<Character>().isDead) FindClosestTarget(Spawner.m_Monsters.ToArray());
        


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
            Invoke("InitAttack", 1.0f);
        }
    }

}

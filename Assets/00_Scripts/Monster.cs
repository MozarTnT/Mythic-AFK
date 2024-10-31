using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float m_Speed;

    Animator animator;

    bool isSpawn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Spawn_Start());
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (isSpawn == false)
            return;


        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if(targetDistance <= 0.5f)
        {
            AnimatorChange("isIDLE");
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimatorChange("isMOVE");
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
            current += Time.deltaTime; // 1�ʸ���
            percent = current / 0.2f;
                           // �������� (����, ��, �ð�) => ���ۺ��� ������ Ư�� �ð� �ӵ��� �̵�
            float LerpPos = Mathf.Lerp(start, end, percent); 
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    private void AnimatorChange(string temp) // �ִϸ����� ��ȯ
    {
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }
}

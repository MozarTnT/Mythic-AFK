using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Range(0f, 100f)]
    public float speed;

    public ParticleSystem Explosion_Particle;
    public Transform Meteor_OBJ;
    public Transform Circle;

    Transform parentTransform;

    public void Init(double dmg)
    {
        if(parentTransform == null)
        {
            parentTransform = transform.parent;
        }

        transform.parent = null;
        StartCoroutine(Meteor_Coroutine(dmg));
    }

    IEnumerator Meteor_Coroutine(double dmg)
    {
        Meteor_OBJ.localPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(10.0f, 12.0f), Random.Range(5.0f, 10.0f));
        Meteor_OBJ.gameObject.SetActive(true);
        Meteor_OBJ.LookAt(transform.parent);

        Circle.localScale = Vector3.one;
        SpriteRenderer renderer = Circle.GetComponent<SpriteRenderer>();

        while(true)
        {
            float distance = Vector3.Distance(Meteor_OBJ.localPosition, Vector3.zero);

            if(distance >= 0.1f)
            {
                Meteor_OBJ.localPosition = Vector3.MoveTowards(Meteor_OBJ.localPosition, Vector3.zero, Time.deltaTime * speed);
                float ScaleValue = distance / speed;

                renderer.color = new Color(0, 0, 0, Mathf.Min((distance / speed), 0.5f)); // 둘 중 작은값 반환
                Circle.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
                yield return null;
            }
            else
            {
                Explosion_Particle.Play();
                Camera_Manager.instance.CameraShake();
                for(int i = 0; i < Spawner.m_Monsters.Count; i++)
                {
                    if(Vector3.Distance(transform.position, Spawner.m_Monsters[i].transform.position) <= 1.5f)
                    {
                        Spawner.m_Monsters[i].GetDamage(dmg);
                    }
                }
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        transform.parent = parentTransform;
        Meteor_OBJ.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}




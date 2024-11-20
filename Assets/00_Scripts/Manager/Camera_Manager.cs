using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    float m_Distance = 4.0f;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float Distance_Value;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Distance(), Time.deltaTime * 2.0f);
    }

    float Distance()
    {
        var players = Spawner.m_Players.ToArray();
        float maxDistance = m_Distance;

        foreach(var player in players)
        {
            float targetDistance = Vector3.Distance(Vector3.zero, player.transform.position) + Distance_Value;
            Debug.Log(targetDistance);


            if(targetDistance > maxDistance) 
            {
                maxDistance = targetDistance;
            }
        }

        return maxDistance;
    }
}

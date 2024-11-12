using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
   
    public Transform COIN;
    [SerializeField] private Transform LAYER;

    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }

}

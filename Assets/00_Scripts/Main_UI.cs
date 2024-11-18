using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null;
    [SerializeField] private TextMeshProUGUI m_Level_Text;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TextCheck();
    }

    public void TextCheck()
    {
        m_Level_Text.text = "Lv." + (Base_Manager.Player.Level + 1).ToString();
    }
  
}

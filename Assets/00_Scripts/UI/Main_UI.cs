using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null;
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_ALLATK_Text;

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
        m_ALLATK_Text.text = StringMethod.ToCurrencyString(Base_Manager.Player.Average_ATK());
    }
  
}

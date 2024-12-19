using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes_Part : MonoBehaviour
{
    [SerializeField] private Image m_Slider, m_CharacterImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;
    [SerializeField] private GameObject GetLock;
    public GameObject LockOBJ;

    public Character_Scriptable m_Character;
    UI_Heroes parent;
    
    public void Initialize(Character_Scriptable data, UI_Heroes parentBase)
    {
        parent = parentBase;
        m_Character = data;
        m_RarityImage.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        m_CharacterImage.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_CharacterImage.SetNativeSize();
        RectTransform rect = m_CharacterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);

        Get_Character_Check();
    }

    public void Get_Character_Check()
    {
        bool Get = false;
        for(int i = 0; i < Base_Manager.Character.m_Set_Character.Length; i++)
        {
            if(Base_Manager.Character.m_Set_Character[i] != null)
            {
                if(Base_Manager.Character.m_Set_Character[i].Data == m_Character)
                {
                    Get = true;
                }
            }
        }

        GetLock.SetActive(Get);
    }

    public void Click_My_Hero()
    {
        parent.Set_Click(this);
        Render_Manager.instance.HERO.GetParticle(true);
    }
}

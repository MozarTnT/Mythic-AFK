using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heroes_Part : MonoBehaviour
{
    [SerializeField] private Image m_Slider, m_CharacterImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;

    Character_Scriptable m_Character;
    
    public void Initialize(Character_Scriptable data)
    {
        m_Character = data;
        m_RarityImage.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        m_CharacterImage.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_CharacterImage.SetNativeSize();
        RectTransform rect = m_CharacterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);
    }

    public void Click_My_Hero()
    {
        Render_Manager.instance.HERO.GetParticle(true);
    }
}

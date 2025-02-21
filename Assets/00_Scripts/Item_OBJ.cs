using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Item_OBJ : MonoBehaviour
{

    [SerializeField] private Transform ItemTextRect;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private GameObject[] Raritys;
    [SerializeField] private ParticleSystem m_Loot;
    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;


    Item_Scriptable m_Item;
    Rarity rarity;

    bool isCheck = false;

    private void Update()
    {
        if(isCheck == false) return;
        if(Base_Canvas.isSave) return;

        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnSave()
    {
        Main_UI.instance.GetItem(m_Item);
        Base_Manager.Inventory.GetItem(m_Item);
        ItemTextRect.gameObject.SetActive(false);

        if(Base_Canvas.isSave)
        {
            Base_Canvas.instance.m_UI.GetComponent<UI_SavingMode>().GetItem(m_Item);
        }

        Base_Manager.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity; // 회전 초기화

        Raritys[((int)rarity)].SetActive(true);

        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.parent = Base_Canvas.instance.HOLDER_LAYER(2);

        m_Text.text = Utils.String_Color_Rarity(rarity) + m_Item.Item_Name + "</color>";

        StartCoroutine(LootItem());
    }

    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for(int i = 0; i < Raritys.Length; i++)
        {
            Raritys[i].SetActive(false);
        }

        ItemTextRect.transform.parent = this.transform;
        ItemTextRect.gameObject.SetActive(false);

        m_Loot.Play();

        Main_UI.instance.GetItem(m_Item);
        Base_Manager.Inventory.GetItem(m_Item);

        if(Base_Canvas.isSave)
        {
            Base_Canvas.instance.m_UI.GetComponent<UI_SavingMode>().GetItem(m_Item);
        }

        yield return new WaitForSeconds(0.5f);

        Base_Manager.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    public void Init(Vector3 pos, Item_Scriptable data)
    {
        m_Item = data;
        rarity = m_Item.rarity;

        UI_SavingMode.m_OnSaving += OnSave;

        isCheck = false;
        transform.position = pos;
        Vector3 target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulateProjectile(target_Pos));
    }

    public void OnDisable()
    {
        UI_SavingMode.m_OnSaving -= OnSave;
    }


    IEnumerator SimulateProjectile(Vector3 pos)
    {
        float target_Distance = Vector3.Distance(transform.position, pos);

        // 목표 지점까지의 거리와 발사 각도, 중력을 기반으로 발사체의 초기 속도를 계산
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // 발사체의 초기 수직 속도를 계산 (발사 속도와 발사 각도를 기반으로)
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float firingDuration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0f;
        while(time < firingDuration) // 발사 지속 시간 동안 반복
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;

            yield return null;
        }
        RarityCheck();
    }

   
}

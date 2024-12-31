using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Item_Scriptable data;
    public int Count;
}
public class Item_Manager
{
    public Dictionary<string, Item_Scriptable> Item_Datas = new Dictionary<string, Item_Scriptable>();

   
    public void Init()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        for(int i = 0; i < datas.Length; i++)
        {
            Item_Datas.Add(datas[i].name, datas[i]);
            // 파이어베이스 DB에서 값 반환 예정
        }
    }

    public List<Item_Scriptable> GetDropSet()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Item_Datas)
        {
            float valueCount = Random.Range(0, 100.0f);

            if(valueCount <= data.Value.Item_Chance)
            {
                objs.Add(data.Value);
            }
        }
        return objs;
    }
}


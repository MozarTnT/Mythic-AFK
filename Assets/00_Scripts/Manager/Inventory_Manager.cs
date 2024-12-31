using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager
{
    public Dictionary<string, Item> m_Items = new Dictionary<string, Item>();

    public void GetItem(Item_Scriptable item)
    {
        if(m_Items.ContainsKey(item.name))
        {
            m_Items[item.name].Count++;
            return;
        }
        m_Items.Add(item.name, new Item { data = item, Count = 1 });
    }
    
}

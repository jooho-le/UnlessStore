using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int Tier { get; private set; }
    [field: SerializeField] public float Scale { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField] private List<Item> items;

    Dictionary<int, Item> cachedItems;
    public int Count => items.Count;

    [ContextMenu("Init")]
    private void Init()
    {
        cachedItems = new Dictionary<int, Item>(items.Count);

        foreach (var item in items)
        {
            if (cachedItems.ContainsKey(item.Id)) continue;

            cachedItems[item.Id] = item;
        }
    }

    public Item GetItem(int id)
    {
        if (cachedItems == null)
        {
            Init();
        }

        return cachedItems.ContainsKey(id) ? cachedItems[id] : null;
    }
}

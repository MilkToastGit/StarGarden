using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Main;

    public InventoryItem[][] AllItems = new InventoryItem[2][];
    [SerializeField] private Decoration[] serialisedAllDecorations;
    [SerializeField] private Hat[] serialisedAllHats;

    private void Awake()
    {
        if (!Main)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddItem(Item item)
    {
        AllItems[item.ItemCategory][item.ItemIndex].Count++;
    }

    public void RemoveItem(Item item)
    {
        if (--AllItems[item.ItemCategory][item.ItemIndex].Count < 0)
            throw new System.Exception($"Could not remove item {item.Name}. Item count is zero");
    }

    public InventoryItem[] GetAllItemsFromCategory(int category) => AllItems[category];

    private void UpdateAllItems()
    {
        if (serialisedAllDecorations.Length <= 0) return;

        Item[][] serialisedAllItems = new Item[2][];
        serialisedAllItems[0] = serialisedAllDecorations;
        serialisedAllItems[1] = serialisedAllHats;

        for (int category = 0; category < 2; category++)
        {
            AllItems[category] = new InventoryItem[serialisedAllItems[category].Length];

            for (int i = 0; i < serialisedAllItems[category].Length; i++)
            {
                AllItems[category][i] = new InventoryItem();
                if (serialisedAllItems[category][i])
                {
                    AllItems[category][i].Item = serialisedAllItems[category][i];
                    AllItems[category][i].Item.ItemIndex = i;
                }
            }
        }
    }
    
    private void OnValidate()
    {
        UpdateAllItems();
    }
}

public class InventoryItem
{
    public Item Item;
    public int Count;
    public int InventoryIndex;
}

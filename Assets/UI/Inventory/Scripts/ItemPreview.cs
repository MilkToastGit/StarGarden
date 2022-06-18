using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPreview : MonoBehaviour
{
    public InventoryItem Item => item;
    private InventoryItem item;

    private TextMeshProUGUI itemCount;

    public void SetItem(InventoryItem item)
    {
        this.item = item;
        itemCount.text = item.Count.ToString();
        Instantiate(item.Item.Prefab, transform.GetChild(0));
    }
}

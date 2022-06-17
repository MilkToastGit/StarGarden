using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPreview : MonoBehaviour
{
    public Item Item => item;
    private Item item;

    public void SpawnItemPrefab(Item item)
    {
        this.item = item;
        Instantiate(item.Prefab, transform.GetChild(0));
    }
}

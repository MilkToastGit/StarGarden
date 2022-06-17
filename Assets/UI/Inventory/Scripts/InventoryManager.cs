using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject itemPreview;
    public InventoryTab[] tabs;

    private int activeTab = 0;

    private void Awake()
    {
        foreach (InventoryTab tab in tabs)
            SpawnItemPreviews(tab);
    }

    public void SwitchTab (int tab)
    {
        tabs[activeTab].Content.gameObject.SetActive(false);
        activeTab = tab;
        tabs[activeTab].Content.gameObject.SetActive(true);
    }

    private void SpawnItemPreviews(InventoryTab tab)
    {
        foreach (Item item in tab.Items)
        {
            GameObject preview = Instantiate(itemPreview, tab.Content);
            preview.GetComponent<ItemPreview>().SpawnItemPrefab(item);
        }
    }
}

[System.Serializable]
public class InventoryTab
{
    public Transform Content;
    public Item[] Items;
}

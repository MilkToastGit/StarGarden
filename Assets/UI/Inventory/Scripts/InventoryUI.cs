using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemPreview;
    [SerializeField] private Transform[] tabs;

    private int activeTab = 0;

    public void Show()
    {

    }

    public void Hide()
    {

    }

    public void SwitchTab (int tab)
    {
        tabs[activeTab].gameObject.SetActive(false);
        activeTab = tab;
        tabs[activeTab].gameObject.SetActive(true);
    }

    private void SpawnItemPreviews(int tab)
    {
        tabs[tab].Order66();

        InventoryItem[] items = InventoryManager.Main.GetAllItemsFromCategory(tab);
        foreach (InventoryItem item in items)
        {
            if (item.Count > 0)
            {
                GameObject preview = Instantiate(itemPreview, tabs[tab]);
                preview.GetComponent<ItemPreview>().SetItem(item);
            }
        }
    }
}

[System.Serializable]
public class InventoryTab
{
    public Transform Content;
    public ItemInstance[] Items;
}

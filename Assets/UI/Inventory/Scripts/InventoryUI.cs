using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Item[] itemsToAdd;

    [SerializeField] private GameObject itemPreview;
    [SerializeField] private Transform[] tabs;

    private int activeTab = 0;
    private GameObject UIBase;

    private void Awake()
    {
        UIBase = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        foreach (Item item in itemsToAdd)
        {
            for (int i = 0; i < Random.Range(0, 6); i++)
                InventoryManager.Main.AddItem(item);
        }
        Show();
    }

    public void Show()
    {
        for (int i = 0; i < 2; i++)
            SpawnItemPreviews(i);
        UIBase.SetActive(true);
    }

    public void Hide()
    {
        UIBase.SetActive(false);
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

        ItemInstances[] items = InventoryManager.Main.GetAllItemsFromCategory(tab);
        foreach (ItemInstances item in items)
        {
            if (item.InventoryCount > 0)
            {
                GameObject preview = Instantiate(itemPreview, tabs[tab]);
                preview.GetComponent<ItemPreview>().SetItem(item);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.UI
{
    public class InventoryUI : UIPanel
    {
        public Item[] itemsToAdd;

        [SerializeField] private GameObject itemPreview, placeableDecor;
        [SerializeField] private Transform[] tabs;

        private int activeTab = 0;

        // **PLACEHOLDER**
        private void Start()
        {
            foreach (Item item in itemsToAdd)
            {
                for (int i = 0; i < Random.Range(1, 6); i++)
                    InventoryManager.Main.AddItem(item);
            }
            //Show();
        }

        public override void Show(object args = null)
        {
            for (int i = 0; i < 2; i++)
                SpawnItemPreviews(i);

            base.Show();
        }

        public void SwitchTab(int tab)
        {
            tabs[activeTab].gameObject.SetActive(false);
            activeTab = tab;
            tabs[activeTab].gameObject.SetActive(true);
        }

        private void OnDecorationClicked(DecorationInstances item)
        {
            InventoryManager.Main.SpawnItem(item, false);

            UIManager.Main.HideCurrentPanel();
        }

        private void OnHatClicked(HatInstances item)
        {

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
                    if (tab == 0)
                        preview.GetComponent<Button>().onClick.AddListener(() => OnDecorationClicked(item as DecorationInstances));
                    else
                        preview.GetComponent<Button>().onClick.AddListener(() => OnHatClicked(item as HatInstances));
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public Item[] itemsToAdd;

        [SerializeField] private GameObject itemPreview, placeableDecor;
        [SerializeField] private Transform[] tabs;

        private int activeTab = 0;
        private GameObject UIBase;
        private bool showing = false;

        private void Awake()
        {
            UIBase = transform.GetChild(0).gameObject;
        }

        // **PLACEHOLDER**
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
            showing = true;
            for (int i = 0; i < 2; i++)
                SpawnItemPreviews(i);
            UIBase.SetActive(true);
        }

        public void Hide()
        {
            showing = false;
            UIBase.SetActive(false);
        }

        public void SwitchTab(int tab)
        {
            tabs[activeTab].gameObject.SetActive(false);
            activeTab = tab;
            tabs[activeTab].gameObject.SetActive(true);
        }

        private void OnDecorationClicked(DecorationInstances item)
        {
            Instantiate(placeableDecor, (Vector2)Camera.main.transform.position, Quaternion.identity)
                .GetComponent<PlaceableDecoration>().SetItem(item, true);

            Hide();
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
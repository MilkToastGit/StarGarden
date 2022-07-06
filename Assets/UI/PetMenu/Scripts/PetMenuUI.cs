using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.UI
{
    public class PetMenuUI : MonoBehaviour
    {
        public static PetMenuUI Main;

        private GameObject UIBase;
        private bool showing;

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            UIBase = transform.GetChild(0).gameObject;
        }

        private void Start()
        {
            ItemInstances[] items = InventoryManager.Main.GetAllItemsFromCategory(0);
            Sprite[] sprites = new Sprite[items.Length];
            for (int i = 0; i < items.Length; i++)
                sprites[i] = items[i].Item.Prefab.GetComponent<SpriteRenderer>().sprite;

            UIManager.Main.ShowSelectionMenu(sprites, OnSelectionMade);
        }

        private void OnSelectionMade(int selectedIndex)
        {
            print(selectedIndex);
        }

        public void Show()
        {
            showing = true;
            UIBase.SetActive(true);
        }

        public void Hide()
        {
            showing = false;
            UIBase.SetActive(false);
        }
    }
}
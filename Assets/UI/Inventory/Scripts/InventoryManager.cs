using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;

namespace StarGarden.Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Main;

        public ItemInstances[][] AllItems = new ItemInstances[2][];
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
            AllItems[item.ItemCategory][item.ItemIndex].IncreaseCount();
        }

        public void RemoveItem(Item item)
        {
            if (AllItems[item.ItemCategory][item.ItemIndex].InventoryCount > 0)
                AllItems[item.ItemCategory][item.ItemIndex].DecreaseCount();
            else throw new System.Exception($"Could not remove item {item.Name}. Item count is zero");
        }

        public ItemInstances[] GetAllItemsFromCategory(int category) => AllItems[category];

        private void UpdateAllItems()
        {
            if (serialisedAllDecorations.Length <= 0) return;

            Item[][] serialisedAllItems = new Item[2][];
            serialisedAllItems[0] = serialisedAllDecorations;
            serialisedAllItems[1] = serialisedAllHats;

            for (int category = 0; category < 2; category++)
            {
                AllItems[category] = new ItemInstances[serialisedAllItems[category].Length];

                for (int i = 0; i < serialisedAllItems[category].Length; i++)
                {
                    AllItems[category][i] = category == 0 ? new DecorationInstances() : new HatInstances();
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
}
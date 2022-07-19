using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Items
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Main;

        public ItemInstances[][] AllItems = new ItemInstances[2][];
        [SerializeField] private Decoration[] serialisedAllDecorations;
        [SerializeField] private Hat[] serialisedAllHats;
        [SerializeField] private GameObject decorationPrefab;

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            //UpdateAllItems();
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
        public ItemInstances GetItemInstance(Item item) => AllItems[item.ItemCategory][item.ItemIndex];

        public void SpawnItem(DecorationInstances item, bool place, Vector2Int point = default)
        {
            if (place)
                Instantiate(decorationPrefab, Core.WorldGrid.GridToWorld(point), Quaternion.identity)
                    .GetComponent<PlaceableDecoration>().SetItem(item, true, point); 
            else
                Instantiate(decorationPrefab, (Vector2)Camera.main.transform.position, Quaternion.identity)
                    .GetComponent<PlaceableDecoration>().SetItem(item, false);
                
        }

        public void UpdateAllItems(ItemSaveData data)
        {
            print("I made it dad");
            AllItems = new ItemInstances[2][];
            AllItems[0] = new ItemInstances[data.Decorations.Length];
            AllItems[1] = new ItemInstances[data.Hats.Length];

            for (int i = 0; i < data.Decorations.Length; i++)
            {
                DecorationSaveData decorData = data.Decorations[i];
                Decoration decorItem = serialisedAllDecorations[decorData.Decoration];
                AllItems[0][i] = new DecorationInstances(decorData, decorItem);
                AllItems[0][i].Item.ItemIndex = i;
                print(AllItems[0][i].Item.Name);

                DecorationInstances instance = AllItems[0][i] as DecorationInstances;
                foreach (Vector2Int point in instance.placedInstances)
                    SpawnItem(instance, true, point);
            }

            for (int i = 0; i < data.Hats.Length; i++)
            {
                HatSaveData hatData = data.Hats[i];
                Hat hatItem = serialisedAllHats[hatData.Hat];
                AllItems[1][i] = new HatInstances(hatData, hatItem);
                AllItems[1][i].Item.ItemIndex = i;
                print(AllItems[1][i].Item.Name);
            }
        }

        //private void UpdateAllItems()
        //{
        //    if (serialisedAllDecorations.Length <= 0) return;

        //    Item[][] serialisedAllItems = new Item[2][];
        //    serialisedAllItems[0] = serialisedAllDecorations;
        //    serialisedAllItems[1] = serialisedAllHats;

        //    for (int category = 0; category < 2; category++)
        //    {
        //        AllItems[category] = new ItemInstances[serialisedAllItems[category].Length];

        //        for (int i = 0; i < serialisedAllItems[category].Length; i++)
        //        {
        //            AllItems[category][i] = category == 0 ? new DecorationInstances() : new HatInstances();
        //            if (serialisedAllItems[category][i])
        //            {
        //                AllItems[category][i].Item = serialisedAllItems[category][i];
        //                AllItems[category][i].Item.ItemIndex = i;
        //            }
        //        }
        //    }
        //}

        private void OnValidate()
        {
            //UpdateAllItems();
        }
    }
}
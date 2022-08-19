using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Items
{
    public static class ItemPicker
    {
        public static ItemInstances PickItem(Rarity rarity, Item excludedItem = null)
        {
            ItemInstances[] pool = InventoryManager.Main.GetAllItemsFromCategory(Random.value > 0.65f ? 1 : 0);

            Debug.Log(rarity);
            //Debug.Log(itemRarity + " " + pool[0].Item.ItemCategory);

            //Vector2Int range = -Vector2Int.one;
            //for (int i = 0; i < pool.Length; i++)
            //{
            //    // if min not set and item matches rarity, set min
            //    if (range.x < 0 && pool[i].Item.Rarity == itemRarity)
            //        range.x = i;
            //    // if min set and item no longer matches rarity, set max
            //    if (range.x >= 0 && pool[i].Item.Rarity != itemRarity)
            //        range.y = i;
            //}
            //if (range.y < 0) range.y = pool.Length;
            //if (range.x < 0) throw new System.Exception($"Error. No {itemRarity} items in the item pool.");

            List<int> possibleIndexes = new List<int>();
            for (int i = 0; i < pool.Length; i++)
                if (pool[i].Item.Rarity == rarity && 
                    (excludedItem == null || pool[i].Item != excludedItem))
                    possibleIndexes.Add(i);

            if (possibleIndexes.Count < 0) throw new System.Exception($"Error. No {rarity} items in the item pool.");

            return pool[possibleIndexes.Random()];
        }

        public static Rarity PickItemRarity(Rarity wishRarity)
        {
            float value = Random.value;
            switch (wishRarity)
            {
                case Rarity.Common:
                    if (value < 0.8f) return Rarity.Common;
                    else return Rarity.Rare;
                case Rarity.Rare:
                    if (value < 0.4f) return Rarity.Common;
                    else if (value < 0.95f) return Rarity.Rare;
                    else return Rarity.Mythical;
                case Rarity.Mythical:
                    if (value < 0.2f) return Rarity.Rare;
                    else return Rarity.Mythical;
            }
            return Rarity.Common;
        }
    }
}
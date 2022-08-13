using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;

namespace StarGarden.Core.SaveData
{
    public class DailyOfferSaveData : MonoBehaviour
    {
        public int Category, Index;
        public bool Purchased;

        public DailyOfferSaveData(int category, int index, bool purchased)
        {
            Category = category;
            Index = index;
            Purchased = purchased;
        }

        public DailyOfferSaveData(Item item, bool purchased)
        {
            Category = item.ItemCategory;
            Index = item.ItemIndex;
            Purchased = purchased;
        }
    }
}

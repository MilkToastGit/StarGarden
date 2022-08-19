using System;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Core.SaveData;

namespace StarGarden.Items
{
    public class DailyOfferGenerator : MonoBehaviour, Manager
    {
        public static DailyOffer[] DailyOffers => dailyOffers;

        private static DailyOffer[] dailyOffers;

        public void Initialise() { }
        public void LateInitialise()
        {
            DateTime lastDate = new DateTime(
                    SaveDataManager.LastSessionSaveDate.Year,
                    SaveDataManager.LastSessionSaveDate.Month,
                    SaveDataManager.LastSessionSaveDate.Day);

            if (lastDate != DateTime.Today || SaveDataManager.SaveData.DailyOfferItems == null)
                PickDailyOffers();
            else
                dailyOffers = SaveDataManager.SaveData.DailyOfferItems;
        }

        private void PickDailyOffers()
        {
            dailyOffers = new DailyOffer[]
            {
                null,
                new DailyOffer(ItemPicker.PickItem(Rarity.Common).Item),
                new DailyOffer(ItemPicker.PickItem(Rarity.Rare).Item),
                new DailyOffer(ItemPicker.PickItem(Rarity.Mythical).Item)
            };

            dailyOffers[0] = new DailyOffer(ItemPicker.PickItem(Rarity.Common, dailyOffers[1].Item).Item);

            SaveDataManager.SaveData.DailyOfferItems = dailyOffers;
            SaveDataManager.SaveAll();
        }

        //public static DailyOffer GetDailyOfferItem(Rarity rarity) => GetDailyOfferItem((int)rarity);
        public static DailyOffer GetDailyOfferItem(int offer)
        {
            DailyOffer item = dailyOffers[offer];
            return new DailyOffer(InventoryManager.Main.AllItems[item.Category][item.Index].Item);
        }

        public static void PurchaseDailyOffer(int offer)
        {
            print(dailyOffers[offer].Item);
            InventoryManager.Main.AddItem(dailyOffers[offer].Item);
            dailyOffers[offer].Purchased = true;
            SaveDataManager.SaveData.DailyOfferItems[offer].Purchased = true;
            SaveDataManager.SaveAll();
        }
    }

    [Serializable]
    public class DailyOffer
    {
        public int Category, Index;
        public bool Purchased;

        public Item Item => InventoryManager.Main.AllItems[Category][Index].Item;

        public DailyOffer(int category, int index, bool purchased = false)
        {
            Category = category;
            Index = index;
            Purchased = purchased;
        }

        public DailyOffer(Item item, bool purchased = false)
        {
            Category = item.ItemCategory;
            Index = item.ItemIndex;
            Purchased = purchased;
        }
    }
}
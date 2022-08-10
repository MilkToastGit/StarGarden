using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.UI
{
    public class DailyOffersUI : MonoBehaviour
    {
        [SerializeField] private Image[] itemPreview;
        [SerializeField] private GameObject[] itemPurchasedIndicator;
        [SerializeField] private int[] dailyOfferCost;

        private void UpdateItems()
        {
            DailyOffer[] offers = DailyOfferGenerator.DailyOffers;

            for (int i = 0; i < itemPreview.Length; i++)
            {
                itemPreview[i].sprite = DailyOfferGenerator.GetDailyOfferItem(i).Item.Sprite;
                itemPurchasedIndicator[i].SetActive(offers[i].Purchased);
            }
        }

        private void OnEnable() => UpdateItems();

        public void PurchaseDailyOffer(int offer)
        {
            if (DailyOfferGenerator.DailyOffers[offer].Purchased) return;
            if (!ResourcesManager.Main.TryPurchase((Rarity)offer, dailyOfferCost[offer]))
            {
                print("NOT ENOUGH STARDUST");
                return;
            }

            DailyOfferGenerator.PurchaseDailyOffer(offer);
            UpdateItems();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;
using TMPro;

namespace StarGarden.UI
{
    public class DailyOffersUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeLeft;
        [SerializeField] private Image[] itemPreview;
        [SerializeField] private GameObject[] itemPurchasedIndicator;
        [SerializeField] private int[] dailyOfferCost;

        private void UpdateUI()
        {
            DailyOffer[] offers = DailyOfferGenerator.DailyOffers;

            for (int i = 0; i < itemPreview.Length; i++)
            {
                itemPreview[i].sprite = DailyOfferGenerator.GetDailyOfferItem(i).Item.Sprite;
                itemPurchasedIndicator[i].SetActive(offers[i].Purchased);
            }

            System.DateTime midnight = System.DateTime.Today.AddDays(1);
            System.TimeSpan tilMidnight = midnight - System.DateTime.Now;
            timeLeft.text = tilMidnight.Hours.ToString();
        }

        private void OnEnable() => UpdateUI();

        public void PurchaseDailyOffer(int offerIndex)
        {
            if (DailyOfferGenerator.DailyOffers[offerIndex].Purchased) return;

            DailyOffer offer = DailyOfferGenerator.DailyOffers[offerIndex];
            Rarity rarity = offer.Item.Rarity;
            print(offer.Item.Rarity);
            if (!ResourcesManager.Main.TryPurchase(rarity, dailyOfferCost[offerIndex]))
            {
                print("NOT ENOUGH STARDUST");
                return;
            }

            DailyOfferGenerator.PurchaseDailyOffer(offerIndex);
            UpdateUI();
        }
    }
}
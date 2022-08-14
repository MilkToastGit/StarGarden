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

        public void PurchaseDailyOffer(int offer)
        {
            if (DailyOfferGenerator.DailyOffers[offer].Purchased) return;
            if (!ResourcesManager.Main.TryPurchase((Rarity)offer, dailyOfferCost[offer]))
            {
                print("NOT ENOUGH STARDUST");
                return;
            }

            DailyOfferGenerator.PurchaseDailyOffer(offer);
            UpdateUI();
        }
    }
}
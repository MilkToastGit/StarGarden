using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.LootBoxes
{
    public class StarWishSequence : MonoBehaviour
    {
        public GameObject uiHolder, puzzleHolder, itemHolder;
        public ConstellationPuzzle puzzle;
        public int commonCost, rareCost, mythicalCost;

        public Image itemPreview;
        public Image itemBackground;

        [SerializeField] private GameObject rerollButton;
        private Rarity wishRarity;
        private Item rolledItem;

        public void MakeWish(int rarity)
        {
            wishRarity = (Rarity)rarity;
            rolledItem = null;
            int cost = 0;

            switch (rarity)
            {
                case 0: cost = commonCost;
                    break;
                case 1: cost = rareCost;
                    break;
                case 2: cost = mythicalCost;
                    break;
            }

            if (!ResourcesManager.Main.TryPurchase(wishRarity, cost))
                return;

            rerollButton.SetActive(true);
            puzzleHolder.SetActive(true);
            uiHolder.SetActive(true);
            puzzle.SetPuzzle(() => RollItem());
        }

        public void RollItem()
        {
            puzzleHolder.SetActive(false);
            uiHolder.SetActive(false);
            itemHolder.SetActive(true);
            rolledItem = ItemPicker.PickItem(wishRarity, rolledItem).Item;
            itemPreview.sprite = rolledItem.Sprite;
            switch (rolledItem.Rarity)
            {
                case Rarity.Common:
                    itemBackground.color = new Color(0.96f, 1f, 0.38f);
                    break;
                case Rarity.Rare:
                    itemBackground.color = new Color(0.9f, 0f, 1f);
                    break;
                case Rarity.Mythical:
                    itemBackground.color = new Color(0f, 1f, 0.85f);
                    break;
            }
        }

        public void CollectItem() => InventoryManager.Main.AddItem(rolledItem);
    }
}
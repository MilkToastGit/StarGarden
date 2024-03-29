using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;
using FMODUnity;

namespace StarGarden.LootBoxes
{
    public class StarWishSequence : MonoBehaviour
    {
        public GameObject uiHolder, puzzleHolder, itemHolder;
        public ConstellationPuzzle puzzle;
        public int commonCost, rareCost, mythicalCost;
        public Item tutorialItem;

        public Image itemPreview;
        public Image itemBackground;

        [SerializeField] private GameObject rerollButton;
        private Rarity wishRarity;
        private Item rolledItem;
        private FMOD.Studio.Bus music;

        private void Awake()
        {
            music = RuntimeManager.GetBus("bus:/Music");
        }

        public void MakeWish(int rarity)
        {
            wishRarity = (Rarity)rarity;
            rolledItem = null;

            if (!ResourcesManager.Main.TryPurchase(wishRarity, GetCost(wishRarity)))
                return;

            rerollButton.SetActive(true);
            puzzleHolder.SetActive(true);
            uiHolder.SetActive(true);
            puzzle.SetPuzzle(() => RollItem());
            music.setVolume(Core.SaveData.SaveDataManager.SaveData.MusicVolume * 0.1f);
        }

        public void RollItem()
        {
            puzzleHolder.SetActive(false);
            uiHolder.SetActive(false);
            itemHolder.SetActive(true);
            rolledItem = ItemPicker.PickItem(ItemPicker.PickItemRarity(wishRarity), rolledItem).Item;
            itemPreview.sprite = rolledItem.Sprite;
            SetBackgroundColour(rolledItem.Rarity);
            music.setVolume(0f);
        }

        public void MakeTutorialWish()
        {
            wishRarity = tutorialItem.Rarity;
            ResourcesManager.Main.TryPurchase(wishRarity, GetCost(wishRarity));

            rerollButton.SetActive(false);
            puzzleHolder.SetActive(true);
            uiHolder.SetActive(true);
            puzzle.SetPuzzle(() => RollTutorialItem());
            music.setVolume(Core.SaveData.SaveDataManager.SaveData.MusicVolume * 0.25f);
        }

        public void RollTutorialItem()
        {
            puzzleHolder.SetActive(false);
            uiHolder.SetActive(false);
            itemHolder.SetActive(true);
            rolledItem = tutorialItem;
            itemPreview.sprite = rolledItem.Sprite;
            SetBackgroundColour(Rarity.Common);
            music.setVolume(0f);
        }

        private void SetBackgroundColour(Rarity rarity)
        {
            switch (rarity)
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

        private int GetCost(Rarity rarity)
        {
            int cost = 0;
            switch ((int)rarity)
            {
                case 0:
                    cost = commonCost;
                    break;
                case 1:
                    cost = rareCost;
                    break;
                case 2:
                    cost = mythicalCost;
                    break;
            }

            return cost;
        }

        public void CollectItem()
        { 
            InventoryManager.Main.AddItem(rolledItem);
            music.setVolume(Core.SaveData.SaveDataManager.SaveData.MusicVolume);
        }

    }
}
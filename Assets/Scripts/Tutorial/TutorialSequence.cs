using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Core.SaveData;

namespace StarGarden.Tutorial
{
    public class TutorialSequence : MonoBehaviour, Manager
    {
        public Tutorial[] tutorials;
        public Pets.PetUnlocker petUnlocker;
        public Stardust.Starfall starfall;
        public UI.UIPanel shopPanel, inventoryPanel, petMenuPanel;
        public Button wishSummonButton, wishCollectButton;

        private int currentTutorial = -1;


        private void Awake()
        {
            print("HERE");
        }
        public void Initialise() { }

        public void LateInitialise() 
        {
            print($"tutorial complete: {SaveDataManager.SaveData.TutorialCompleted}");
            if (!SaveDataManager.SaveData.TutorialCompleted)
                petUnlocker.OnFinalPetUnlocked += OnFinalPetUnlocked;
        }

        private void OnFinalPetUnlocked()
        {
            print("Final pet unlocked");
            petUnlocker.OnFinalPetUnlocked -= NextTutorial;
            NextTutorial();
        }

        private void NextTutorial()
        {
            CompleteCurrent();
            if (++currentTutorial == tutorials.Length)
                return;

            tutorials[currentTutorial].OnLastSlideReached += OnTutorialReachedLastSlide;
            tutorials[currentTutorial].Show();
        }

        private void CompleteCurrent()
        {
            if (!currentTutorial.Between(0, tutorials.Length))
                return;

            tutorials[currentTutorial].Hide();
            tutorials[currentTutorial].OnLastSlideReached -= OnTutorialReachedLastSlide;
        }

        private void OnTutorialReachedLastSlide(Tutorial tutorial)
        {
            switch (currentTutorial)
            {
                case 0: // Introduction | wait for zoom into island
                    IslandManager.Main.OnActiveIslandChanged += OnActiveIslandChanged;
                    break;
                case 1: // Acknowledge Pet
                    InputManager.Main.OnTapCompleted += OnTapCompleted1;
                    break;
                case 2: // Spawn Starfall | wait for starfall lant
                    starfall.OnLanded += OnStarfallLanded;
                    break;
                case 3: // Acknowledge starfall | wait for starfall collect
                    starfall.OnCollected += OnStarfallCollected;
                    break;
                case 4: // Explain stardust | wait for shop to open
                    shopPanel.OnShow += OnShopShow;
                    break;
                case 5: // Explain store | wait for wish collect button press
                    // complete current on wish button press
                    wishSummonButton.onClick.AddListener(OnWishSummonButtonClick);
                    wishCollectButton.onClick.AddListener(OnWishCollectButtonClick);
                    break;
                case 6: // Acknowledge decoration prize | wait for close shop
                    shopPanel.OnHide += OnShopHide;
                    break;
                case 7: // Acknowledge inventory | wait for inventory open
                    inventoryPanel.OnShow += OnInventoryShow;
                    break;
                case 8: // Explain placing items | wait for inventory close
                    inventoryPanel.OnHide += OnInventoryHide;
                    break;
                case 9: // Explain placing items 2, prompt to tap pet | wait for pet menu
                    petMenuPanel.OnShow += OnPetMenuShow;
                    break;
                case 10: // Explain pet menu
                    InputManager.Main.OnTapCompleted += OnTapCompleted10;
                    break;
            }
        }

        private void OnActiveIslandChanged(int island)
        {
            NextTutorial();
            IslandManager.Main.OnActiveIslandChanged -= OnActiveIslandChanged;
        }

        private void OnTapCompleted1(Vector2 pos)
        {
            NextTutorial();
            InputManager.Main.OnTapCompleted -= OnTapCompleted1;
        }

        private void OnTapCompleted10(Vector2 pos)
        {
            CompleteCurrent();
            InputManager.Main.OnTapCompleted -= OnTapCompleted10;
            SaveDataManager.SaveData.TutorialCompleted = true;
            SaveDataManager.SaveAll();
        }

        private void OnStarfallLanded()
        {
            NextTutorial();
            starfall.OnLanded -= OnStarfallLanded;
        }

        private void OnStarfallCollected(Stardust.Starfall s)
        {
            NextTutorial();
            starfall.OnCollected -= OnStarfallCollected;
        }

        private void OnShopShow()
        {
            NextTutorial();
            shopPanel.OnShow -= OnShopShow;
        }

        private void OnWishSummonButtonClick()
        {
            CompleteCurrent();
            wishSummonButton.onClick.RemoveListener(OnWishSummonButtonClick);
        }

        private void OnWishCollectButtonClick()
        {
            NextTutorial();
            wishCollectButton.onClick.RemoveListener(OnWishCollectButtonClick);
        }

        private void OnShopHide()
        {
            NextTutorial();
            shopPanel.OnHide -= OnShopHide;
        }

        private void OnInventoryShow()
        {
            NextTutorial();
            inventoryPanel.OnShow -= OnInventoryShow;
        }

        private void OnInventoryHide()
        {
            NextTutorial();
            inventoryPanel.OnHide -= OnInventoryHide;
        }

        private void OnPetMenuShow()
        {
            NextTutorial();
            petMenuPanel.OnShow -= OnPetMenuShow;
        }
    }
}
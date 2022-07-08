using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;
using StarGarden.Pets;
using UnityEngine.UI;
using TMPro;

namespace StarGarden.UI
{
    public class PetMenuUI : MonoBehaviour, UIPanel
    {
        public static PetMenuUI Main;
        [SerializeField] private TextMeshProUGUI petName, personalityText;
        [SerializeField] private Image petImage, hatImage, signImage;
        [SerializeField] private HappinessBar happinessBar;

        private GameObject UIBase;
        private WanderingPet[] pets;
        private int currentPet;
        private bool showing;

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            UIBase = transform.GetChild(0).gameObject;
            pets = PetManager.Main.GetActivePets();
        }

        public void ShowHatSelectMenu()
        {
            ItemInstances[] items = InventoryManager.Main.GetAllItemsFromCategory(1);
            Sprite[] sprites = new Sprite[items.Length];
            for (int i = 0; i < items.Length; i++)
                sprites[i] = items[i].Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;

            UIManager.Main.ShowSelectionMenu(sprites, OnHatSelected);
        }

        private void OnHatSelected(int selectedIndex)
        {
            HatInstances hat = InventoryManager.Main.GetAllItemsFromCategory(1)[selectedIndex] as HatInstances;
            hat.Equip(pets[currentPet]);
            pets[currentPet].SetHat(hat);
            hatImage.sprite = hat.Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }

        public void SetPet(int pet)
        {
            if (pet == currentPet) return;

            currentPet = F.Wrap(pet, 0, pets.Length);

            //Transform hatParent;
            //pets[currentPet].

            petName.text = pets[currentPet].Pet.Name;
            petImage.sprite = pets[currentPet].Pet.Sprite;
            signImage.sprite = pets[currentPet].Pet.SignSprite;
            personalityText.text = pets[currentPet].Pet.PersonalityTraits;
            if (pets[currentPet].EquippedHat != null)
                hatImage.sprite = pets[currentPet].EquippedHat.Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
            else hatImage.sprite = null;
            happinessBar.SetHappiness(pets[currentPet].Happiness);
        }

        public void FeedCookie(bool isCommon)
        {
            float amount = isCommon ? 0.75f : 0.15f;
            pets[currentPet].IncreaseHappiness(amount);
            happinessBar.SetHappiness(pets[currentPet].Happiness);
        }

        public void NextPet() => SetPet(currentPet + 1);
        public void PreviousPet() => SetPet(currentPet - 1);

        public void Show()
        {
            SetPet(currentPet);

            showing = true;
            UIBase.SetActive(true);
        }

        public void Hide()
        {
            showing = false;
            UIBase.SetActive(false);
        }
    }
}
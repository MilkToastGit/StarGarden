using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;
using StarGarden.Pets;
using UnityEngine.UI;
using TMPro;
using StarGarden.Pets;

namespace StarGarden.UI
{
    public class PetMenuUI : MonoBehaviour, UIPanel
    {
        public static PetMenuUI Main;
        [SerializeField] private TextMeshProUGUI petName, personalityText;
        [SerializeField] private Image petImage, hatImage, signImage;

        private GameObject UIBase;
        private WanderingPet selectedPet;
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
            Debug.Log(UIBase.name);
        }

        public void ShowHatSelectMenu()
        {
            ItemInstances[] items = InventoryManager.Main.GetAllItemsFromCategory(1);
            Sprite[] sprites = new Sprite[items.Length];
            for (int i = 0; i < items.Length; i++)
                sprites[i] = items[i].Item.Prefab.GetComponent<SpriteRenderer>().sprite;

            UIManager.Main.ShowSelectionMenu(sprites, OnHatSelected);
        }

        private void OnHatSelected(int selectedIndex)
        {
            HatInstances hat = InventoryManager.Main.GetAllItemsFromCategory(1)[selectedIndex] as HatInstances;
            hat.Equip(selectedPet);
            selectedPet.SetHat(hat);
            hatImage.sprite = hat.Item.Prefab.GetComponent<SpriteRenderer>().sprite;
        }

        public void SetPet(WanderingPet pet)
        {
            petName.text = pet.Pet.Name;
            petImage.sprite = pet.Pet.Sprite;
            signImage.sprite = pet.Pet.SignSprite;
            personalityText.text = pet.Pet.PersonalityTraits;
            if (pet.EquippedHat != null)
                hatImage.sprite = pet.EquippedHat.Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
            selectedPet = pet;
        }

        public void Show()
        {
            //if (pet) SetPet(pet);
            //else SetPet(PetManager.Main.GetActivePets()[0]);

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
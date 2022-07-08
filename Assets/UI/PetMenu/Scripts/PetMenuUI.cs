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
        [SerializeField] private Sprite defaultHatSprite;
        [SerializeField] private Transform hatParent;

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

        public void SetPet(int petIndex)
        {
            if (petIndex == currentPet) return;

            currentPet = F.Wrap(petIndex, 0, pets.Length);
            WanderingPet pet = pets[currentPet];

            petImage.sprite = pet.Pet.Sprite;

            if (pet.EquippedHat != null)
            {
                //Sprite petSprite = pet.Pet.Sprite;
                //(hatParent as RectTransform).pivot = (Vector3)(petSprite.pivot / petSprite.rect.size);

                hatImage.sprite = pet.EquippedHat.Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                //hatParent.Order66();
                //for (int i = 0; i < pet.DefaultHatPositions.Length; i++)
                //{
                //    GameObject hat = new GameObject("hat", typeof(RectTransform), typeof(Image));
                //    hat.GetComponent<Image>().sprite = pet.EquippedHat.Item.Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                //    //Transform spawnedHat = Instantiate(pet.EquippedHat.Item.Prefab, hatParent).transform;
                //    Transform spawnedHat = hat.transform;
                //    spawnedHat.SetParent(hatParent);
                //    spawnedHat.localPosition = pet.DefaultHatPositions[i];
                //    spawnedHat.localRotation = pet.DefaultHatRotations[i];
                //    spawnedHat.localScale = pet.DefaultHatScales[i];
                //}
            }
            else hatImage.sprite = defaultHatSprite;

            petName.text = pet.Pet.Name;
            signImage.sprite = pet.Pet.SignSprite;
            personalityText.text = pet.Pet.PersonalityTraits;
            happinessBar.SetHappiness(pet.Happiness);
        }

        public void FeedCookie(bool isCommon)
        {
            float amount = isCommon ? 0.1f : 0.175f;
            pets[currentPet].IncreaseHappiness(amount);
            happinessBar.SetHappiness(pets[currentPet].Happiness);
        }

        // Placeholder
        public void ResetHappiness()
        {
            pets[currentPet].Happiness = 0f;
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
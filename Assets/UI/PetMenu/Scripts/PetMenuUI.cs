using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;
using StarGarden.Pets;
using UnityEngine.UI;
using TMPro;

namespace StarGarden.UI
{
    public class PetMenuUI : UIPanel
    {
        public static PetMenuUI Main;
        [SerializeField] private TextMeshProUGUI petName, negativeTrait, neutralTrait, positiveTrait, horroscope;
        [SerializeField] private Image petImage, hatImage, signImage;
        [SerializeField] private HappinessBar happinessBar;
        [SerializeField] private Sprite defaultHatSprite;
        [SerializeField] private Transform hatParent;

        private PetInstance[] pets => PetManager.Main.AllActivePets;
        private int currentPet = -1;

        public override void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            base.Initialise();
        }

        public override void LateInitialise()
        {
            base.LateInitialise();
        }

        public override void Show(object petIndex)
        {
            if (petIndex.GetType() == typeof(int))
                SetPet((int)petIndex);
            base.Show();
        }
        
        public void ShowHatSelectMenu()
        {
            ItemInstances[] items = InventoryManager.Main.GetAllItemsFromCategory(1);
            //Sprite[] sprites = new Sprite[items.Length];
            //for (int i = 0; i < items.Length; i++)
            //    sprites[i] = items[i].Item.Sprite;

            UIManager.Main.ShowSelectionMenu(items, OnHatSelected);
        }

        private void OnHatSelected(int selectedIndex)
        {
            if (selectedIndex < 0)
            {
                HatInstances hat = pets[currentPet].WanderingPet.EquippedHat;
                if (hat == null) return;

                hat.Unequip(pets[currentPet].WanderingPet);
                hatImage.sprite = defaultHatSprite;
            }
            else
            {
                HatInstances hat = InventoryManager.Main.GetAllItemsFromCategory(1)[selectedIndex] as HatInstances;
                hat.Equip(pets[currentPet].WanderingPet);
                hatImage.sprite = hat.Item.Sprite;
            }
        }

        public void SetPet(int petIndex)
        {
            if (petIndex == currentPet) return;

            currentPet = F.Wrap(petIndex, 0, pets.Length);
            WanderingPet pet = pets[currentPet].WanderingPet;
            print(pet);

            print(pets[0]);
            petImage.sprite = pet.Pet.Sprite;

            if (pet.EquippedHat != null)
            {
                //Sprite petSprite = pet.Pet.Sprite;
                //(hatParent as RectTransform).pivot = (Vector3)(petSprite.pivot / petSprite.rect.size);

                hatImage.sprite = pet.EquippedHat.Item.Sprite;
                //hatParent.Order66();
                //for (int i = 0; i < pet.DefaultHatPositions.Length; i++)
                //{
                //    GameObject hat = new GameObject("hat", typeof(RectTransform), typeof(Image));
                //    hat.GetComponent<Image>().sprite = pet.EquippedHat.Item.Sprite;
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

            negativeTrait.text = pet.Pet.NegativeTrait;
            neutralTrait.text = pet.Pet.NeutralTrait;
            positiveTrait.text = pet.Pet.PositiveTrait;

            horroscope.text = HorroscopeGenerator.GetHorroscope(pet.Pet.Starsign);

            happinessBar.SetHappiness(pet.Happiness);
        }

        public void FeedCookie(bool isCommon)
        {
            if (!Core.ResourcesManager.Main.TryPurchase(isCommon ? Rarity.Common : Rarity.Rare, 1) ||
                pets[currentPet].WanderingPet.Happiness == 1)
                return;

            float amount = isCommon ? 0.2f : 0.4f;
            pets[currentPet].WanderingPet.IncreaseHappiness(amount);
            happinessBar.SetHappiness(pets[currentPet].WanderingPet.Happiness);
        }

        // Placeholder
        public void ResetHappiness()
        {
            pets[currentPet].WanderingPet.SetHappiness(0f);
            happinessBar.SetHappiness(pets[currentPet].WanderingPet.Happiness);
        }

        public void NextPet() => SetPet(currentPet + 1);
        public void PreviousPet() => SetPet(currentPet - 1);
    }
}
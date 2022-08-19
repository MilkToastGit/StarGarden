using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Pets;
using StarGarden.Core.SaveData;

namespace StarGarden.Pets
{
    public class PetUnlocker : MonoBehaviour, Manager
    {
        public delegate void FinalPetUnlockedEvent();
        public FinalPetUnlockedEvent OnFinalPetUnlocked;

        [SerializeField] private Button PetUnlockCloseButton;

        private Queue<Starsign> petsToUnlock = new Queue<Starsign>();

        public void Initialise() { }
        public void LateInitialise()
        {
            StartCoroutine(WaitForBirthdayInput());
        }

        private IEnumerator WaitForBirthdayInput()
        {
            yield return new WaitUntil(() => SaveDataManager.SaveData.UserBirthdate != default);
            GetPetsToUnlock();
            if (petsToUnlock.Count > 0)
                UnlockNextPet();
            else
                OnFinalPetUnlocked?.Invoke();
        }

        private void GetPetsToUnlock()
        {
            Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
            Starsign currentStarsign = Zodiac.GetStarsignFromDate(System.DateTime.Today);
            if (currentStarsign == userStarsign)
                currentStarsign = Zodiac.Zodiacs[F.Wrap((int)currentStarsign - 1, 0, Zodiac.Zodiacs.Length)].Starsign;

            if (!PetManager.Main.GetPetFromStarsign(userStarsign).Obtained)
                petsToUnlock.Enqueue(userStarsign);
            if (!PetManager.Main.GetPetFromStarsign(currentStarsign).Obtained)
                petsToUnlock.Enqueue(currentStarsign);
        }

        private void UnlockNextPet()
        {
            PetManager.UnlockPet(petsToUnlock.Dequeue());
            if (petsToUnlock.Count > 0)
                PetUnlockCloseButton.onClick.AddListener(UnlockNextPet);
            else
            {
                PetUnlockCloseButton.onClick.RemoveListener(UnlockNextPet);
                PetUnlockCloseButton.onClick.AddListener(FinalPetCollected);
            }
        }

        private void FinalPetCollected()
        {
            OnFinalPetUnlocked?.Invoke();
            PetUnlockCloseButton.onClick.RemoveListener(FinalPetCollected);
        }

        //public void UnlockBirthPet()
        //{
        //    Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
        //    //PetManager.UnlockPet(userStarsign);
        //    PetManager.UnlockPet(PetManager.Main.AllPets.Random().Pet.Starsign); // **PLACEHOLDER**
        //    PetUnlockCloseButton.onClick.AddListener(UnlockCurrentPet);
        //}

        //public void UnlockCurrentPet()
        //{
        //    Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
        //    Starsign currentStarsign = Zodiac.GetStarsignFromDate(System.DateTime.Today);
        //    if (currentStarsign == userStarsign)
        //        currentStarsign = Zodiac.Zodiacs[F.Wrap((int)currentStarsign - 1, 0, Zodiac.Zodiacs.Length)].Starsign;

        //    //PetManager.UnlockPet(currentStarsign);
        //    PetManager.UnlockPet(PetManager.Main.AllPets.Random().Pet.Starsign); // **PLACEHOLDER**

        //    PetUnlockCloseButton.onClick.RemoveListener(UnlockCurrentPet);
        //}
    }
}

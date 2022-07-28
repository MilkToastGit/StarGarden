using UnityEngine;
using StarGarden.Pets;
using StarGarden.Core.SaveData;
using UnityEngine.UI;

namespace StarGarden.Pets
{
    public class UnlockInitialPets : MonoBehaviour
    {
        [SerializeField] private Button PetUnlockCloseButton;

        public void UnlockBirthPet()
        {
            Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
            //PetManager.UnlockPet(userStarsign);
            PetManager.UnlockPet(Starsign.Leo); // **PLACEHOLDER**
            PetUnlockCloseButton.onClick.AddListener(UnlockCurrentPet);
        }

        public void UnlockCurrentPet()
        {
            Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
            Starsign currentStarsign = Zodiac.GetStarsignFromDate(System.DateTime.Today);
            if (currentStarsign == userStarsign)
                currentStarsign = Zodiac.Zodiacs[F.Wrap((int)currentStarsign - 1, 0, Zodiac.Zodiacs.Length)].Starsign;

            //PetManager.UnlockPet(currentStarsign);
            PetManager.UnlockPet(Starsign.Gemini); // **PLACEHOLDER**

            PetUnlockCloseButton.onClick.RemoveListener(UnlockCurrentPet);
        }
    }
}

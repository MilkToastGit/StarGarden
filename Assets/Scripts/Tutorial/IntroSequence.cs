using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Pets;
using StarGarden.Core.SaveData;

namespace StarGarden.Tutorial
{
    public class IntroSequence : MonoBehaviour
    {
        public void UnlockBirthPet()
        {
            Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
            PetManager.UnlockPet(userStarsign);
            //PetManager.Main.GetPetFromStarsign(userStarsign).Pet.Sprite;
            print(userStarsign);
        }

        public void UnlockCurrentPet()
        {
            Starsign userStarsign = Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate);
            Starsign currentStarsign = Zodiac.GetStarsignFromDate(System.DateTime.Today);
            if (currentStarsign == userStarsign)
                currentStarsign = Zodiac.Zodiacs[F.Wrap((int)currentStarsign - 1, 0, Zodiac.Zodiacs.Length)].Starsign;

            PetManager.UnlockPet(currentStarsign);

            print(currentStarsign);
        }
    }
}

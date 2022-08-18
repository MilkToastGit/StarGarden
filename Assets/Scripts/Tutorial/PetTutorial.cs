using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;
using StarGarden.Core;
using StarGarden.Pets;

namespace StarGarden.Tutorial
{
    public class PetTutorial : Tutorial
    {
        public Tutorial PetMenuTutorial;

        public override void Show()
        {
            base.Show();
            StartCoroutine(TutorialSequence());
        }

        private void OnPetMenuActivated(Tutorial tutorial)
        {
            Hide();
            PetMenuTutorial.OnActivated -= OnPetMenuActivated;
        }

        private IEnumerator TutorialSequence()
        {
            PetInstance pet = PetManager.Main.GetPetFromStarsign(Zodiac.GetStarsignFromDate(SaveDataManager.SaveData.UserBirthdate));
            Island island = IslandManager.Main.GetIslandFromElement(pet.Pet.Element);

            yield return new WaitUntil(() => IslandManager.Main.ActiveIsland == island);
            NextSlide();

            PetMenuTutorial.OnActivated += OnPetMenuActivated;
        }
    }
}
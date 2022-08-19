using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Pets;

namespace StarGarden.Tutorial
{
    public class IslandSelectionEnforcement : MonoBehaviour
    {
        public Camera cam;
        //public GameObject button;
        public SpotlightSetter spotlightSetter;
        public Collider2D[] islandColliders;
        public GameObject[] tutorialTexts;

        private void OnEnable()
        {
            PetInstance pet = PetManager.Main.GetPetFromStarsign(Zodiac.GetStarsignFromDate(Core.SaveData.SaveDataManager.SaveData.UserBirthdate));
            Island island = IslandManager.Main.GetIslandFromElement(pet.Pet.Element);
            SetIsland(island);
        }

        public void SetIsland(Island island)
        {
            for (int i = 0; i < islandColliders.Length; i++)
                if (i != island.Index)
                {
                    islandColliders[i].enabled = false;
                    tutorialTexts[i].SetActive(false);
                }

            IslandManager.Main.OnActiveIslandChanged += EnableIslandColliders;

            //button.transform.position = islandPos;
            Vector2 islandPos = island.IslandNavigationObject.transform.position;
            islandPos = cam.WorldToScreenPoint(islandPos);
            islandPos.y = Screen.height - islandPos.y;
            //spotlightCentre.position = island.IslandNavigationObject.transform.position;
            spotlightSetter.spotlightCentre = islandPos;
            spotlightSetter.SetProperties();
        }

        private void EnableIslandColliders(int island)
        {
            foreach(Collider2D collider in islandColliders)
                collider.enabled = true;
        }
    }
}
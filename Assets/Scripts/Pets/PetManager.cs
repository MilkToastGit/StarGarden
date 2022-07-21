using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    public class PetManager : MonoBehaviour
    {
        public static PetManager Main;

        public PetInstance[] AllPets;
        public float CollectiveHappiness { get { float h = 0f; foreach (PetInstance pet in AllPets) h += pet.WanderingPet.Happiness; return h / AllPets.Length; } }

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            for (int i = 0; i < AllPets.Length; i++)
            {
                AllPets[i].Pet.PetIndex = i;
                if (AllPets[i].Obtained)
                {
                    Transform island = Core.IslandManager.Main.Islands[AllPets[i].Island].IslandObject.transform;
                    AllPets[i].WanderingPet = Instantiate(AllPets[i].Pet.Prefab, island).GetComponent<WanderingPet>();
                }
            }
        }

        public WanderingPet[] GetActivePets()
        {
            List<WanderingPet> pets = new List<WanderingPet>();
            for (int i = 0; i < AllPets.Length; i++)
            {
                if(AllPets[i].Island >= 0)
                    pets.Add(AllPets[i].WanderingPet);
            }

            return pets.ToArray();
        }
    }
}
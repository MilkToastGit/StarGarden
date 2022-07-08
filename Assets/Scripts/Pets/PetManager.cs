using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    public class PetManager : MonoBehaviour
    {
        public static PetManager Main;

        public PetInstance[] AllPets;

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            for (int i = 0; i < AllPets.Length; i++)
                AllPets[i].Pet.Pet.PetIndex = i;
        }

        public WanderingPet[] GetActivePets()
        {
            List<WanderingPet> pets = new List<WanderingPet>();
            for (int i = 0; i < AllPets.Length; i++)
            {
                if(AllPets[i].Island >= 0)
                    pets.Add(AllPets[i].Pet);
            }

            return pets.ToArray();
        }
    }
}
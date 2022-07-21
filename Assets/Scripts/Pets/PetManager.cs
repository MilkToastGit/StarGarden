using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    public class PetManager : MonoBehaviour, Manager
    {
        public static PetManager Main;

        [HideInInspector] public PetInstance[] AllPets { get; private set; }
        [SerializeField] private Pet[] serialisedAllPets;
        public float CollectiveHappiness { get { float h = 0f; foreach (PetInstance pet in AllPets) if (pet.Obtained) h += pet.WanderingPet.Happiness; return h / AllPets.Length; } }

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void LateInitialise() { }

        public void UpdateAllPets()
        {
            AllPets = new PetInstance[serialisedAllPets.Length];

            for (int i = 0; i < serialisedAllPets.Length; i++)
            {
                AllPets[i] = new PetInstance();
                AllPets[i].Pet = serialisedAllPets[i];
                AllPets[i].Pet.PetIndex = i;
                AllPets[i].Obtained = true; // ** PLACEHOLDER **
            }
        }

        public void UpdateAllPets(Core.SaveData.PetSaveData[] data)
        {
            AllPets = new PetInstance[serialisedAllPets.Length];

            for (int i = 0; i < serialisedAllPets.Length; i++)
            {
                AllPets[i] = new PetInstance(data[i], serialisedAllPets[i]);
                AllPets[i].Pet.PetIndex = i;

                if (AllPets[i].Obtained)
                {
                    Transform island = Core.IslandManager.Main.Islands[AllPets[i].Island].IslandObject.transform;
                    AllPets[i].WanderingPet = Instantiate(AllPets[i].Pet.Prefab, island).GetComponent<WanderingPet>();
                    AllPets[i].WanderingPet.Initialise();
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
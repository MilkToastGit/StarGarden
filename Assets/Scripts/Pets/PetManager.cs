using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Pets
{
    public class PetManager : MonoBehaviour, Manager
    {
        public static PetManager Main;

        public PetInstance[] AllPets { get; private set; }
        public PetInstance[] AllActivePets { get; private set; }
        [SerializeField] private Pet[] serialisedAllPets;
        public float CollectiveHappiness { get {
                int active = 0;
                float h = 0f; 
                foreach (PetInstance pet in AllPets)
                    if (pet.Obtained)
                    {
                        h += pet.WanderingPet.Happiness;
                        active++;
                    }
                return h / active; 
            } 
        }

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void LateInitialise() 
        {
            PetSaveData[] data = SaveDataManager.SaveData.PetSaveData;
            if (data == null)
                UpdateAllPets();
            else
                UpdateAllPets(data);
        }

        public void UpdateAllPets()
        {
            AllPets = new PetInstance[serialisedAllPets.Length];

            for (int i = 0; i < serialisedAllPets.Length; i++)
            {
                AllPets[i] = new PetInstance();
                AllPets[i].Pet = serialisedAllPets[i];
                AllPets[i].Pet.PetIndex = i;
                //AllPets[i].Obtained = true; // ** PLACEHOLDER **
            }
        }

        public void UpdateAllPets(PetSaveData[] data)
        {
            AllPets = new PetInstance[serialisedAllPets.Length];

            for (int i = 0; i < serialisedAllPets.Length; i++)
            {
                AllPets[i] = new PetInstance(data[i], serialisedAllPets[i]);
                AllPets[i].Pet.PetIndex = i;

                if (AllPets[i].Obtained)
                    SpawnPet(AllPets[i], CalculateNewHappiness(data[i].Happiness));
            }

            UpdateActivePets();
            SaveDataManager.SavePetData();
        }

        private float CalculateNewHappiness(float initialHappiness)
        {
            System.TimeSpan sinceLast = System.DateTime.Now - SaveDataManager.SaveData.LastSave;
            float decrease = F.Map((float)sinceLast.TotalHours, 0f, 48f);
            //print($"Happiness Decrease: {initialHappiness} - {decrease} = {initialHappiness - decrease}");
            return Mathf.Max(initialHappiness - decrease, 0);
        }

        public void UpdateActivePets()
        {
            List<PetInstance> pets = new List<PetInstance>();
            for (int i = 0; i < AllPets.Length; i++)
            {
                if(AllPets[i].Obtained)
                    pets.Add(AllPets[i]);
            }

            AllActivePets = pets.ToArray();
        }

        public PetInstance GetPetFromStarsign(Starsign sign)
        {
            foreach (PetInstance pet in AllPets)
                if (pet.Pet.Starsign == sign)
                    return pet;
            throw new System.Exception($"Error: Pet of sign {sign} does not exist.");
        }

        public static void UnlockPet(Starsign sign) => UnlockPet(Main.GetPetFromStarsign(sign));
        public static void UnlockPet(PetInstance pet)
        {
            pet.Obtained = true;
            SpawnPet(pet, 0.5f);
            Main.UpdateActivePets();
            SaveDataManager.SavePetData();
            UI.UIManager.Main.ShowPanel("PetUnlocker", pet.Pet);
        }

        public static void SpawnPet(PetInstance pet, float happiness)
        {
            Transform island = Core.IslandManager.Main.GetIslandFromElement(pet.Pet.Element).IslandObject.transform;
            pet.WanderingPet = Instantiate(pet.Pet.Prefab, island).GetComponent<WanderingPet>();
            pet.WanderingPet.Initialise(happiness);
            //print("w" + pet.WanderingPet);
        }
    }
}
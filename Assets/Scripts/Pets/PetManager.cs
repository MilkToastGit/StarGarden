using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

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

        public void LateInitialise() 
        {
            AllSaveData data = SaveDataManager.SaveData;
            if (data == null)
                UpdateAllPets();
            else
                UpdateAllPets(data.PetSaveData);
        }

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

        public void UpdateAllPets(PetSaveData[] data)
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
                    float newHappiness = CalculateNewHappiness(data[i].Happiness, SaveDataManager.SaveData.LastSave);
                    AllPets[i].WanderingPet.Initialise(newHappiness);
                }
            }
        }

        private float CalculateNewHappiness(float initialHappiness, System.DateTime lastSave)
        {
            System.TimeSpan sinceLast = System.DateTime.Now - lastSave;
            float decrease = F.Map((float)sinceLast.TotalHours, 0f, 48f);
            print($"Happiness Decrease: {initialHappiness} - {decrease} = {initialHappiness - decrease}");
            return Mathf.Max(initialHappiness - decrease, 0);
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
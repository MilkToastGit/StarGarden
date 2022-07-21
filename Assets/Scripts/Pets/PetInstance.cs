using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    [System.Serializable]
    public class PetInstance
    {
        public Pet Pet;
        public WanderingPet WanderingPet;
        public bool Obtained;
        public int Island;

        public PetInstance() { }

        public PetInstance(Core.SaveData.PetSaveData data, Pet pet)
        {
            Pet = pet;
            Obtained = data.Obtained;
            Island = data.Island;
        }
    }
}
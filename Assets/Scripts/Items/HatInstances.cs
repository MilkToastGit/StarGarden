using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Pets;
using StarGarden.Core.SaveData;

namespace StarGarden.Items
{
    public class HatInstances : ItemInstances
    {
        public override int InventoryCount => totalCount - equippedInstances.Count;
        public List<WanderingPet> equippedInstances { get; private set; } = new List<WanderingPet>();

        public HatInstances() { }

        public HatInstances(HatSaveData data, Hat item)
        {
            Item = item;
            totalCount = data.TotalCount;
            //equippedInstances = new List<WanderingPet>();
            //foreach (int index in data.EquippedInstances)
            //    equippedInstances.Add(PetManager.Main.AllPets[index].WanderingPet);
        }

        public void Equip(WanderingPet pet, bool save = true)
        {
            if (pet.EquippedHat != null)
                pet.EquippedHat.Unequip(pet);
            equippedInstances.Add(pet);
            if(save)
                SaveDataManager.SaveItemData();
            pet.SetHat(this);
        }

        public void Unequip(WanderingPet pet)
        {
            equippedInstances.Remove(pet);
            SaveDataManager.SaveItemData();
            pet.SetHat(null);
        }
    }
}
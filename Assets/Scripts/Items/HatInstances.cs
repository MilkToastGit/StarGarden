using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Pets;

namespace StarGarden.Items
{
    public class HatInstances : ItemInstances
    {
        public override int InventoryCount => totalCount - equippedInstances.Count;
        private List<WanderingPet> equippedInstances = new List<WanderingPet>();

        public void Equip(WanderingPet pet)
        {
            if (pet.EquippedHat != null)
                pet.EquippedHat.Unequip(pet);
            pet.EquippedHat = this;
            equippedInstances.Add(pet);
        }

        public void Unequip(Pet pet)
        {
            equippedInstances.Remove(pet);
        }
    }
}
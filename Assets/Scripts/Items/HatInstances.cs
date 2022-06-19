using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatInstances : ItemInstances
{
    public override int InventoryCount => totalCount - equippedInstances.Count;
    private List<int> equippedInstances = new List<int>();

    public void Equip(int character)
    {
        equippedInstances.Add(character);
    }

    public void Unequip(int character)
    {
        equippedInstances.Remove(character);
    }
}

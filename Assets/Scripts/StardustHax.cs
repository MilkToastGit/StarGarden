using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

public class StardustHax : MonoBehaviour
{
    public void AddCommon(int amount) => ResourcesManager.Main.AddStardust(Rarity.Common, amount);
    public void AddRare(int amount) => ResourcesManager.Main.AddStardust(Rarity.Rare, amount);
    public void AddMythical(int amount) => ResourcesManager.Main.AddStardust(Rarity.Mythical, amount);
}

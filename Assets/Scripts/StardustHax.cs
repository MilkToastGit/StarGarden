using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

public class StardustHax : MonoBehaviour
{
    public void AddStardust(int rarity)
    {
        switch (rarity)
        {
            case 0: ResourcesManager.Main.AddStardust(Rarity.Common, 1);
                break;
            case 1: ResourcesManager.Main.AddStardust(Rarity.Rare, 1);
                break;
            case 2: ResourcesManager.Main.AddStardust(Rarity.Mythical, 1);
                break;
        }
    }
}

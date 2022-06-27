using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Items;

public class PlaceholderItemRoller : MonoBehaviour
{
    public void RollItem(int rarity)
    {
        Rarity r;
        if (rarity == 0)
            r = Rarity.Common;
        else if (rarity == 1)
            r = Rarity.Rare;
        else r = Rarity.Mythical;

        ItemInstances item = ItemPicker.PickItem(r);
        InventoryManager.Main.AddItem(item.Item);
        print($"You Got a {item.Item.Rarity.ToString()} {item.Item.Name}!");
    }
}

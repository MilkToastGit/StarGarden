using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.LootBoxes
{
    public class StarWishSequence : MonoBehaviour
    {
        public ConstellationPuzzle puzzle;
        public int commonCost, rareCost, mythicalCost;

        public SpriteRenderer itemPreview;

        public void MakeWish(Rarity rarity)
        {
            int cost = 0;
            int held = 0;

            switch (rarity)
            {
                case Rarity.Common:
                    cost = commonCost;
                    held = ResourcesManager.Main.CommonStardust;
                    break;
                case Rarity.Rare:
                    cost = rareCost;
                    held = ResourcesManager.Main.RareStardust;
                    break;
                case Rarity.Mythical:
                    cost = mythicalCost;
                    held = ResourcesManager.Main.MythicalStardust;
                    break;
            }

            if (cost > held)
                print("NOT ENOUGH STARDUST");
            else
                ResourcesManager.Main.RemoveStardust(rarity, cost);

            puzzle.gameObject.SetActive(true);
            puzzle.SetPuzzle(() => OnPuzzleCompleted(rarity));
        }

        private void OnPuzzleCompleted(Rarity rarity)
        {
            puzzle.gameObject.SetActive(false);
            ItemInstances item = RollItem(((int)rarity));
            itemPreview.sprite = item.Item.Sprite;
        }

        private ItemInstances RollItem(int rarity)
        {
            Rarity r;
            if (rarity == 0)
                r = Rarity.Common;
            else if (rarity == 1)
                r = Rarity.Rare;
            else r = Rarity.Mythical;

            ItemInstances item = ItemPicker.PickItem(r);
            InventoryManager.Main.AddItem(item.Item);
            print($"You Got a {item.Item.Rarity} {item.Item.Name}!");
            
            return item;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.LootBoxes
{
    public class StarWishSequence : MonoBehaviour
    {
        public GameObject puzzleHolder, itemHolder;
        public ConstellationPuzzle puzzle;
        public int commonCost, rareCost, mythicalCost;

        public Image itemPreview;
        public Image itemBackground;

        public void MakeWish(int rarity)
        {
            Rarity boxRarity = Rarity.Common;
            int cost = 0;
            int held = 0;

            switch (rarity)
            {
                case 0:
                    boxRarity = Rarity.Common;
                    cost = commonCost;
                    held = ResourcesManager.Main.CommonStardust;
                    break;
                case 1:
                    boxRarity = Rarity.Rare;
                    cost = rareCost;
                    held = ResourcesManager.Main.RareStardust;
                    break;
                case 2:
                    boxRarity = Rarity.Mythical;
                    cost = mythicalCost;
                    held = ResourcesManager.Main.MythicalStardust;
                    break;
            }

            if (cost > held)
                print("NOT ENOUGH STARDUST");
            else
                ResourcesManager.Main.RemoveStardust(boxRarity, cost);

            puzzleHolder.SetActive(true);
            puzzle.SetPuzzle(() => OnPuzzleCompleted(boxRarity));
        }

        private void OnPuzzleCompleted(Rarity rarity)
        {
            puzzleHolder.SetActive(false);
            itemHolder.SetActive(true);
            ItemInstances item = RollItem((int)rarity);
            itemPreview.sprite = item.Item.Sprite;
            switch (rarity)
            {
                case Rarity.Common:
                    itemBackground.color = new Color(0.96f, 1f, 0.38f);
                    break;
                case Rarity.Rare:
                    itemBackground.color = new Color(0.9f, 0f, 1f);
                    break;
                case Rarity.Mythical:
                    itemBackground.color = new Color(0f, 1f, 0.85f);
                    break;
            }
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
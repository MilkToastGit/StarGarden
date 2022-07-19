using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;

namespace StarGarden.Core.SaveData
{
    public class GameSetup : MonoBehaviour
    {
        private void Awake()
        {
            SetupItems();
        }

        private void SetupItems()
        {
            ItemSaveData data = SaveDataManager.ReadItemData();
            if (data == null) return;

            InventoryManager.Main.UpdateAllItems(data);
        }
    }
}
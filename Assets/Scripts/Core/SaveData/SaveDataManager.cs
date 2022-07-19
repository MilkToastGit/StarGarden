using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StarGarden.Items;

namespace StarGarden.Core.SaveData
{
    public static class SaveDataManager
    {
        private static readonly string itemsDataPath = "/itemsData.dat";

        public static void SaveItemData()
        {
            ItemInstances[][] AllItems = InventoryManager.Main.AllItems;
            ItemSaveData data = new ItemSaveData();
            foreach(ItemInstances[] items in AllItems)
                data.AddItems(items);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + itemsDataPath);
            bf.Serialize(file, data);
            file.Close();
        }

        public static ItemSaveData ReadItemData()
        {
            if (!File.Exists(Application.persistentDataPath + itemsDataPath))
                throw new System.Exception("Fuck You");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + itemsDataPath, FileMode.Open);
            ItemSaveData data = (ItemSaveData)bf.Deserialize(file);
            file.Close();
            return data;
        }
    }
}
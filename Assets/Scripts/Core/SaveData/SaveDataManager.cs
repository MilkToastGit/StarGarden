using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StarGarden.Items;
using StarGarden.Pets;

namespace StarGarden.Core.SaveData
{
    public static class SaveDataManager
    {
        private static readonly string dataPath = "/gameData.dat";

        public static AllSaveData SaveData => allData;
        private static AllSaveData allData = new AllSaveData();

        public static void SaveItemData()
        {
            ItemInstances[][] allItems = InventoryManager.Main.AllItems;
            ItemSaveData data = new ItemSaveData();
            foreach(ItemInstances[] items in allItems)
                data.AddItems(items);

            allData.ItemSaveData = data;
            WriteSaveData();
        }

        public static void SavePetData()
        {
            PetInstance[] allPets = PetManager.Main.AllPets;
            PetSaveData[] data = new PetSaveData[allPets.Length];
            for (int i = 0; i < allPets.Length; i++)
                data[i] = new PetSaveData(allPets[i]);

            allData.PetSaveData = data;
            WriteSaveData();
        }

        public static void SaveResourceData()
        {
            int common = ResourcesManager.Main.CommonStardust;
            int rare = ResourcesManager.Main.RareStardust;
            int mythical = ResourcesManager.Main.MythicalStardust;
            ResourceSaveData data = new ResourceSaveData(common, rare, mythical);

            allData.ResourceSaveData = data;
            WriteSaveData();
        }

        public static void UpdateAndSaveAll()
        {
            SaveItemData();
            SavePetData();
            SaveResourceData();

            WriteSaveData();
        }

        public static AllSaveData ReadAll() => allData = (AllSaveData)ReadDataFromFile(dataPath);

        private static void WriteSaveData()
        {
            allData.LastSave = System.DateTime.Now;
            allData.LastSaveVersion = Application.version;
            WriteDataToFile(allData, dataPath);
        }

        private static void WriteDataToFile(object data, string subPath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + subPath);
            bf.Serialize(file, data);
            file.Close();
        }

        private static object ReadDataFromFile(string subPath)
        {
            if (!File.Exists(Application.persistentDataPath + subPath))
                return null; 
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + subPath, FileMode.Open);
            object data = bf.Deserialize(file);
            file.Close();
            return data;
        }
    }
}
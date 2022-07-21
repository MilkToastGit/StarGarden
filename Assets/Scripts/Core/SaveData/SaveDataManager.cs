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
        private static readonly string itemsDataPath = "/itemsData.dat";
        private static readonly string petsDataPath = "/petsData.dat";

        public static void SaveItemData()
        {
            ItemInstances[][] allItems = InventoryManager.Main.AllItems;
            ItemSaveData data = new ItemSaveData();
            foreach(ItemInstances[] items in allItems)
                data.AddItems(items);

            WriteDataToFile(data, itemsDataPath);
        }

        public static ItemSaveData ReadItemData() => (ItemSaveData)ReadDataFromFile(itemsDataPath);

        public static void SavePetData()
        {
            PetInstance[] allPets = PetManager.Main.AllPets;
            PetSaveData[] data = new PetSaveData[allPets.Length];
            for (int i = 0; i < allPets.Length; i++)
                data[i] = new PetSaveData(allPets[i]);

            WriteDataToFile(data, petsDataPath);
        }

        public static PetSaveData[] ReadPetData() => (PetSaveData[])ReadDataFromFile(petsDataPath);

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;
using StarGarden.Pets;

namespace StarGarden.Core.SaveData
{
    public class GameSetup : MonoBehaviour
    {
        public GameObject[] ManagerObjects;
        private Manager[] Managers;

        private void Awake()
        {
            GetManagers();
            InitialiseAll();
            LoadSave();
            LateInitialiseAll();
        }

        private void GetManagers()
        {
            Managers = new Manager[ManagerObjects.Length];
            for (int i = 0; i < ManagerObjects.Length; i++)
                Managers[i] = ManagerObjects[i].GetComponent<Manager>();
        }

        private void InitialiseAll()
        {
            foreach (Manager m in Managers)
                m.Initialise();
        }

        private void LateInitialiseAll()
        {
            foreach (Manager m in Managers)
                m.LateInitialise();
        }

        private void LoadSave()
        {
            AllSaveData data = SaveDataManager.ReadAll();
            SetupPets(data == null ? null : data.PetSaveData);
            SetupItems(data == null ? null : data.ItemSaveData);
            SetupResources(data == null ? null : data.ResourceSaveData);
        }

        private void SetupPets(PetSaveData[] data)
        {
            if (data == null)
                PetManager.Main.UpdateAllPets();
            else
                PetManager.Main.UpdateAllPets(data);
        }

        private void SetupItems(ItemSaveData data)
        {
            if (data == null)
                InventoryManager.Main.UpdateAllItems();
            else
                InventoryManager.Main.UpdateAllItems(data);
        }

        private void SetupResources(ResourceSaveData data)
        {
            if (data != null)
                ResourcesManager.Main.UpdateResources(data);
        }
    }
}
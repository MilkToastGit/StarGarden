using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;
using StarGarden.Pets;

namespace StarGarden.Core.SaveData
{
    public class GameSetup : MonoBehaviour
    {
        public GameObject[] InitialiseOrder;
        public GameObject[] LateInitialiseOrder;
        private Manager[] Managers;
        private Manager[] LateInitManagers;

        private void Awake()
        {
            GetManagers();
            InitialiseAll();
            LoadSave();
            LateInitialiseAll();
        }

        private void GetManagers()
        {
            Managers = new Manager[InitialiseOrder.Length];
            for (int i = 0; i < InitialiseOrder.Length; i++)
                Managers[i] = InitialiseOrder[i].GetComponent<Manager>();

            LateInitManagers = new Manager[LateInitialiseOrder.Length];
            for (int i = 0; i < LateInitialiseOrder.Length; i++)
                LateInitManagers[i] = LateInitialiseOrder[i].GetComponent<Manager>();
        }

        private void InitialiseAll()
        {
            foreach (Manager m in Managers)
                m.Initialise();
        }

        private void LateInitialiseAll()
        {
            foreach (Manager m in LateInitManagers)
                m.LateInitialise();
        }

        private void LoadSave()
        {
            AllSaveData data = SaveDataManager.ReadAll();
            if (data.FirstLaunch == default || 
                data.UserBirthdate == default || 
                data.PetSaveData == null || 
                data.PetSaveData.Length < 2)
            {
                SaveDataManager.ResetSaveData();
                SaveDataManager.SaveData.FirstLaunch = System.DateTime.Now;
                UI.UIManager.Main.ShowIntro();
                // TutorialManager.Main.ActivateTutorial();
            }
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
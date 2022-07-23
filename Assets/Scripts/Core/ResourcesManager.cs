using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Core
{
    public class ResourcesManager : MonoBehaviour, Manager
    {
        public int CommonStardust => commonStardust;
        public int RareStardust => rareStardust;
        public int MythicalStardust => mythicalStardust;
        private int commonStardust, rareStardust, mythicalStardust;

        public static ResourcesManager Main;

        public delegate void StardustChangedEvent();
        public event StardustChangedEvent OnStardustChanged;

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void LateInitialise() 
        {
            AllSaveData data = SaveDataManager.SaveData;
            if (data != null)
                UpdateResources(data.ResourceSaveData);
        }

        public void RemoveStardust(Rarity rarity, int amount) => AddStardust(rarity, -amount);

        public void UpdateResources(ResourceSaveData data)
        {
            commonStardust = data.CommonStardust;
            rareStardust = data.RareStardust;
            mythicalStardust = data.MythicalStardust;
        }

        public void AddStardust(Rarity rarity, int amount)
        {
            switch (rarity)
            {
                case Rarity.Common: commonStardust += amount; break;
                case Rarity.Rare: rareStardust += amount; break;
                case Rarity.Mythical: mythicalStardust += amount; break;
            }

            OnStardustChanged?.Invoke();
        }
    }
}
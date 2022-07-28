using System;

namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class AllSaveData
    {
        public PetSaveData[] PetSaveData;
        public ItemSaveData ItemSaveData;
        public ResourceSaveData ResourceSaveData;
        public DateTime FirstLaunch;
        public DateTime UserBirthdate;
        public DateTime LastSave;
        public string LastSaveVersion;

        public AllSaveData() { }

        public AllSaveData(PetSaveData[] petSaveData, ItemSaveData itemSaveData, ResourceSaveData resourceSaveData)
        {
            PetSaveData = petSaveData;
            ItemSaveData = itemSaveData;
            ResourceSaveData = resourceSaveData;
        }
    }
}
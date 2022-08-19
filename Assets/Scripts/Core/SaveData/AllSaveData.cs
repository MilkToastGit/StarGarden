using System;

namespace StarGarden.Core.SaveData
{
    [Serializable]
    public class AllSaveData
    {
        public PetSaveData[] PetSaveData;
        public ItemSaveData ItemSaveData;
        public ResourceSaveData ResourceSaveData;
        public bool TutorialCompleted = false;
        public Items.DailyOffer[] DailyOfferItems;
        public int[] PickedHorroscopes;
        public DateTime FirstLaunch;
        public DateTime UserBirthdate;
        public DateTime LastSave;
        public DateTime AutoCollectExpiry;
        public TimeSpan LastAutoCollectSpan;
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
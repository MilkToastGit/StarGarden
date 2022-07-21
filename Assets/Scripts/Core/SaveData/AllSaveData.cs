namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class AllSaveData
    {
        public PetSaveData[] PetSaveData;
        public ItemSaveData ItemSaveData;
        public ResourceSaveData ResourceSaveData;

        public AllSaveData() { }

        public AllSaveData(PetSaveData[] petSaveData, ItemSaveData itemSaveData, ResourceSaveData resourceSaveData)
        {
            PetSaveData = petSaveData;
            ItemSaveData = itemSaveData;
            ResourceSaveData = resourceSaveData;
        }
    }
}
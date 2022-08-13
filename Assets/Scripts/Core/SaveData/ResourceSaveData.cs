namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class ResourceSaveData
    {
        public int CommonStardust;
        public int RareStardust;
        public int MythicalStardust;

        public ResourceSaveData(int common, int rare, int mythical)
        {
            CommonStardust = common;
            RareStardust = rare;
            MythicalStardust = mythical;
        }
    }
}
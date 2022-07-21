using StarGarden.Pets;

namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class PetSaveData
    {
        public int Pet;
        public bool Obtained;
        public int Island;

        public PetSaveData(PetInstance instance)
        {
            Pet = instance.Pet.PetIndex;
            Obtained = instance.Obtained;
            Island = instance.Island;
        }
    }
}
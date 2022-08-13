using StarGarden.Pets;

namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class PetSaveData
    {
        public int Pet;
        public bool Obtained;
        public float Happiness;
        public int Island;

        public PetSaveData(PetInstance instance)
        {
            Pet = instance.Pet.PetIndex;
            Obtained = instance.Obtained;
            if (Obtained)
                Happiness = instance.WanderingPet.Happiness;
            Island = instance.Island;
        }
    }
}
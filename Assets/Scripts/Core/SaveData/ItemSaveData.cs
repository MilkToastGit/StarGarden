using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Items;

namespace StarGarden.Core.SaveData
{
    [System.Serializable]
    public class ItemSaveData
    {
        public DecorationSaveData[] Decorations;
        public HatSaveData[] Hats;

        public void AddItems(ItemInstances[] items)
        {
            if (items.Length <= 0) return;

            System.Type type = items[0].GetType();
            if (type == typeof(DecorationInstances))
            {
                DecorationSaveData[] data = new DecorationSaveData[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    DecorationInstances item = items[i] as DecorationInstances;
                    data[i] = new DecorationSaveData();
                    data[i].Decoration = item.Item.ItemIndex;
                    data[i].TotalCount = item.totalCount;

                    List<SerializableVector3Int> placedInstances = new List<SerializableVector3Int>();
                    foreach (Vector3Int placed in item.placedInstances)
                        placedInstances.Add(new SerializableVector3Int(placed));
                    data[i].PlacedInstances = placedInstances;
                    //Debug.Log(data[i].TotalCount);
                }
                Decorations = data;
            }
            else if (type == typeof(HatInstances))
            {
                HatSaveData[] data = new HatSaveData[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    HatInstances item = items[i] as HatInstances;
                    data[i] = new HatSaveData();
                    data[i].Hat = item.Item.ItemIndex;
                    data[i].TotalCount = item.totalCount;

                    List<int> equppedInstances = new List<int>();
                    foreach (Pets.WanderingPet pet in item.equippedInstances)
                        equppedInstances.Add(pet.Pet.PetIndex);
                    data[i].EquippedInstances = equppedInstances;
                }
                Hats = data;
            }
        }
    }

    [System.Serializable]
    public class DecorationSaveData
    {
        [SerializeReference] public int Decoration;
        public int TotalCount;
        public List<SerializableVector3Int> PlacedInstances;
    }

    [System.Serializable]
    public class HatSaveData
    {
        [SerializeReference] public int Hat;
        public int TotalCount;
        public List<int> EquippedInstances;
    }

    [System.Serializable]
    public class SerializableVector3Int
    {
        public int x, y, z;

        public SerializableVector3Int(Vector3Int v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static implicit operator Vector3Int(SerializableVector3Int v) => new Vector3Int(v.x, v.y, v.z);
    }
}
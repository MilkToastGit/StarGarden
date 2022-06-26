using UnityEngine;

namespace StarGarden.Items
{
    public abstract class Item : ScriptableObject
    {
        public abstract int ItemCategory { get; }

        public string Name;
        public GameObject Prefab;
        public Rarity Rarity;

        [HideInInspector] public int ItemIndex;
    }
}
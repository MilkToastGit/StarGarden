using UnityEngine;

namespace StarGarden.Items
{
    public abstract class Item : ScriptableObject
    {
        public abstract int ItemCategory { get; }

        public string Name;
        public Rarity Rarity;
        public Sprite Sprite;
        public GameObject Prefab;

        [HideInInspector] public int ItemIndex;
    }
}
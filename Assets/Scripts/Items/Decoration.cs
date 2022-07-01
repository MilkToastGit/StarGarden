using UnityEngine;

namespace StarGarden.Items
{
    [CreateAssetMenu(fileName = "Decor", menuName = "StarGarden/Item/Decoration")]
    public class Decoration : Item
    {
        public override int ItemCategory => 0;
        public Vector2Int Size = Vector2Int.one;
        public Element Element;
    }
}
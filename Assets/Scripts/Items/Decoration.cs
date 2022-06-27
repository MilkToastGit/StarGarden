using UnityEngine;

namespace StarGarden.Items
{
    [CreateAssetMenu(fileName = "Decor", menuName = "StarGarden/Item/Decoration")]
    public class Decoration : Item
    {
        public override int ItemCategory => 0;
        public Element Element;
    }
}
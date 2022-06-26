using UnityEngine;

namespace StarGarden.Items
{
    [CreateAssetMenu(fileName = "Hat", menuName = "StarGarden/Item/Hat")]
    public class Hat : Item
    {
        public override int ItemCategory => 1;
    }
}
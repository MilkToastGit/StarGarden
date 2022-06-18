using UnityEngine;

[CreateAssetMenu (fileName = "Decor", menuName = "StarGarden/Item/Decoration")]
public class Decoration : Item
{
    public override int ItemCategory => 0;
    public Element Element;
    public DecorationCategory Category;

    public override ItemInstance CreateInstance() => new DecorationInstance(this);
}

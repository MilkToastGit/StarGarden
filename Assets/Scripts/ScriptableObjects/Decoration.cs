using UnityEngine;

[CreateAssetMenu (fileName = "Decor", menuName = "StarGarden/Item/Decoration")]
public class Decoration : Item
{
    public Element Element;
    public DecorationCategory Category;
}

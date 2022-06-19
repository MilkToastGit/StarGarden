using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationInstances : ItemInstances
{
    public override int InventoryCount => totalCount - placedInstances.Count;
    private List<Vector2Int> placedInstances = new List<Vector2Int>();

    public void Place(Vector2Int position)
    {
        placedInstances.Add(position);
    }

    public void Unplace(Vector2Int position)
    {
        placedInstances.Remove(position);
    }
}

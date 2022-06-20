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
        Debug.Log(placedInstances.Count);
    }

    public void Unplace(Vector2Int position)
    {
        placedInstances.Remove(position);
    }

    public void Move(Vector2Int initialPosition, Vector2Int newPosition)
    {
        placedInstances[placedInstances.FindIndex(v => v == initialPosition)] = newPosition;
    }

    public bool IsPositionTaken(Vector2Int position) => placedInstances.FindIndex(p => p == position) >= 0;
    //public bool IsPositionTaken(Vector2Int position)
    //{
    //    Debug.Log(placedInstances.FindIndex(p => p == position));
    //    return placedInstances.FindIndex(p => p == position) < 0;
    //}
}

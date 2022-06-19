using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInstances
{
    public Item Item;
    public abstract int InventoryCount { get; }
    protected int totalCount;

    public void IncreaseCount() => totalCount++;

    public void DecreaseCount() => totalCount--;
}

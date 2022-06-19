using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableDecoration : MonoBehaviour, Touchable
{
    public bool Passthrough => false;

    private DecorationInstances decor;
    private bool touched = false, dragging = false;

    private Vector2Int lastPlacedPosition;

    private void Start()
    {
        SetItem(InventoryManager.Main.GetAllItemsFromCategory(0)[0] as DecorationInstances);
    }

    public void SetItem(DecorationInstances decor)
    {
        this.decor = decor;
        Instantiate(decor.Item.Prefab, transform.GetChild(0));
    }

    private void Place(Vector2Int position)
    {
        decor.Place(position);
        print(decor.InventoryCount);
    }

    private void ReturnToInventory()
    {
        decor.Unplace(lastPlacedPosition);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (dragging)
        {
            transform.position = WorldGrid.GridToWorld(WorldGrid.WorldToGrid(InputManager.Main.WorldTouchPosition));
        }
    }

    public void OnStartTouch()
    {
        touched = true;
    }

    public void OnEndTouch()
    {
        if (dragging)
            Place(WorldGrid.WorldToGrid(InputManager.Main.WorldTouchPosition));

        touched = false;
        dragging = false;
    }

    private void OnHoldTouch()
    {
        if (touched)
            dragging = true;
    }

    private void OnEnable()
    {
        InputManager.Main.OnHoldTouch += pos => OnHoldTouch();
    }

    private void OnDisable()
    {
        InputManager.Main.OnStartTouch -= pos => OnHoldTouch();
    }
}

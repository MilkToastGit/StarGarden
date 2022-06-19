using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPreview : MonoBehaviour
{
    public ItemInstances Item => item;
    private ItemInstances item;

    private TextMeshProUGUI itemCount;
    private bool touched = false;

    private void Setup()
    {
        itemCount = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(ItemInstances item)
    {
        Setup();

        this.item = item;
        itemCount.text = item.InventoryCount.ToString();
        Instantiate(item.Item.Prefab, transform.GetChild(0));
    }

    //public void OnPointerDown()
    //{
    //    touched = true;
    //    print("started touch");
    //}

    //public void OnEndTouch()
    //{
    //    touched = false;
    //    print("ended touch");
    //}

    //private void OnHoldTouch()
    //{
    //    if (touched)
    //        print("dragging");
    //}

    //private void OnEnable()
    //{
    //    InputManager.Main.OnHoldTouch += pos => OnHoldTouch();
    //    InputManager.Main.OnEndTouch += pos => OnEndTouch();
    //}
    
    //private void OnDisable()
    //{
    //    InputManager.Main.OnStartTouch -= pos => OnHoldTouch();
    //    InputManager.Main.OnEndTouch -= pos => OnEndTouch();
    //}
}

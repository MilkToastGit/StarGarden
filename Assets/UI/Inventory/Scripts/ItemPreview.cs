using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarGarden.Items;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class ItemPreview : MonoBehaviour
    {
        public ItemInstances Item => item;
        private ItemInstances item;

        [SerializeField] private Image itemImage;
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
            itemImage.sprite = item.Item.Sprite;
            //SpriteRenderer prefabSprite = Instantiate(item.Item.Prefab, transform.GetChild(0)).GetComponentInChildren<SpriteRenderer>();
            //prefabSprite.sortingLayerName = "Menus";
            //prefabSprite.sortingOrder = 1;
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
}
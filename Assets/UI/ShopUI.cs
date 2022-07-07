using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.UI
{
    public class ShopUI : MonoBehaviour, UIPanel
    {
        private GameObject UIBase;
        private bool showing = false;

        private void Awake()
        {
            UIBase = transform.GetChild(0).gameObject;
        }

        public void Show()
        {
            showing = true;
            UIBase.SetActive(true);
        }

        public void Hide()
        {
            showing = false;
            UIBase.SetActive(false);
        }
    }
}
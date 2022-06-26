using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class TabManager : MonoBehaviour
    {
        public InventoryUI inventoryManager;
        public Image[] tabs;
        public Color activeColor, inactiveColor;

        private int activeTab;

        public void SwitchTab(int tab)
        {
            tabs[activeTab].color = inactiveColor;
            activeTab = tab;
            tabs[activeTab].color = activeColor;

            int siblingIndex = tabs.Length - 1;
            int tabIndex = tab;

            while (tabIndex >= 0)
                tabs[tabIndex--].transform.SetSiblingIndex(siblingIndex--);
            tabIndex = tab + 1;
            while (tabIndex < tabs.Length)
                tabs[tabIndex++].transform.SetSiblingIndex(siblingIndex--);

            inventoryManager.SwitchTab(tab);
        }
    }
}
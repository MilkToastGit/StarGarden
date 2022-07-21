using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class UIManager : MonoBehaviour, Manager
    {
        public static UIManager Main;
        public bool PanelShowing => activePanel >= 0;
        public GameObject[] PanelObjects;
        public delegate void SelectionCompleted(int selectedIndex);

        private int activePanel = -1;
        private UIPanel[] panels;
        [SerializeField] private Transform selectionMenuBase;
        [SerializeField] private GameObject selectionItemPreview;
        private Transform selectionMenuItems;

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            selectionMenuItems = selectionMenuBase.GetChild(0);

            panels = new UIPanel[PanelObjects.Length];
            for (int i = 0; i < PanelObjects.Length; i++)
                panels[i] = PanelObjects[i].GetComponent<UIPanel>();
        }
        
        public void LateInitialise() { }

        public void ShowPetMenu(int petIndex)
        {
            PetMenuUI.Main.SetPet(petIndex);
            ShowPanel(2);
        }

        public void ShowSelectionMenu(Sprite[] lst, SelectionCompleted onCompleted)
        {
            selectionMenuItems.Order66();
            selectionMenuBase.gameObject.SetActive(true);

            for (int i = 0; i < lst.Length; i++)
            {
                int index = i;
                GameObject preview = Instantiate(selectionItemPreview, selectionMenuItems);
                preview.GetComponent<Image>().sprite = lst[i];
                preview.GetComponent<Button>().onClick.AddListener(() => onCompleted(index));
                preview.GetComponent<Button>().onClick.AddListener(() => HideSelectionMenu());
            }
        }
        
        public void ShowSelectionMenu(Items.ItemInstances[] lst, SelectionCompleted onCompleted)
        {
            selectionMenuItems.Order66();

            bool atLeastOneSpawned = false;
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i].InventoryCount <= 0) continue;

                atLeastOneSpawned = true;
                int index = i;
                GameObject preview = Instantiate(selectionItemPreview, selectionMenuItems);
                preview.GetComponent<ItemPreview>().SetItem(lst[i]);
                preview.GetComponent<Button>().onClick.AddListener(() => onCompleted(index));
                preview.GetComponent<Button>().onClick.AddListener(() => HideSelectionMenu());
            }

            if (atLeastOneSpawned)
                selectionMenuBase.gameObject.SetActive(true);
            else
                onCompleted(-1);
        }

        public void ShowPanel(int panel)
        {
            if (activePanel >= 0)
                panels[activePanel].Hide();

            activePanel = panel;
            panels[activePanel].Show();
        }

        public void HideCurrentPanel()
        {
            if (activePanel >= 0)
                panels[activePanel].Hide();
            activePanel = -1;
            print(activePanel);
        }

        private void HideSelectionMenu()
        {
            selectionMenuBase.gameObject.SetActive(false);
        }
    }
}
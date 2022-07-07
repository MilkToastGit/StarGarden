using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class UIManager : MonoBehaviour
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

        private void Awake()
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

        public void ShowPetMenu(Pets.WanderingPet pet)
        {
            PetMenuUI.Main.SetPet(pet);
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
        }

        private void HideSelectionMenu()
        {
            selectionMenuBase.gameObject.SetActive(false);
        }
    }
}
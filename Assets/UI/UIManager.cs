using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class UIManager : MonoBehaviour, Manager
    {
        public static UIManager Main;
        public bool PanelShowing => activePanel != "";
        public GameObject[] PanelObjects;
        public delegate void SelectionCompleted(int selectedIndex);

        private Dictionary<string, UIPanel> UIPanels = new Dictionary<string, UIPanel>();
        private string activePanel = "";

        [SerializeField] private GameObject selectionMenuBase;
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
            selectionMenuItems = selectionMenuBase.transform.GetChild(0);

            foreach (GameObject panelGO in PanelObjects)
            {
                UIPanel panel = panelGO.GetComponent<UIPanel>();
                panel.Initialise();
                UIPanels.Add(panelGO.name, panel);
            }
        }
        
        public void LateInitialise() 
        {
            foreach(KeyValuePair<string, UIPanel> pair in UIPanels)
                pair.Value.LateInitialise();
        }

        public void ShowSelectionMenu(Sprite[] lst, SelectionCompleted onCompleted)
        {
            selectionMenuItems.Order66();
            selectionMenuBase.SetActive(true);

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
            {
                selectionMenuBase.SetActive(true);
            }
            else
                onCompleted(-1);
        }

        public void ShowPanel(string name) => ShowPanel(name, null);

        public void ShowPanel(string name, object args)
        {
            if (PanelShowing)
                UIPanels[activePanel].Hide();

            activePanel = name;
            UIPanels[activePanel].Show(args);
        }

        public void HideCurrentPanel()
        {
            if (PanelShowing)
                UIPanels[activePanel].Hide();
            activePanel = "";
        }

        private void HideSelectionMenu()
        {
            selectionMenuBase.SetActive(false);
        }
    }
}
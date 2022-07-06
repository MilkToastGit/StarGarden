using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Main;
        public delegate void SelectionCompleted(int selectedIndex);
        
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

        private void HideSelectionMenu()
        {
            selectionMenuBase.gameObject.SetActive(false);
        }
    }
}
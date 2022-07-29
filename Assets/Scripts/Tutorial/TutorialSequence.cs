using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class TutorialSequence : MonoBehaviour
    {
        public static bool TutorialActive = false;

        public bool Seen => seen;
        public UIPanel ActivatePanel;

        private GameObject baseObject;
        private GameObject[] slides;
        private int currentSlide;
        private bool seen = false;

        private void Awake()
        {
            if (!TutorialActive) return;

            ActivatePanel.OnShow += Show;

            baseObject = transform.GetChild(0).gameObject;

            slides = new GameObject[baseObject.transform.childCount - 1];
            for (int i = 0; i < baseObject.transform.childCount - 1; i++)
            {
                slides[i] = baseObject.transform.GetChild(i).gameObject;
                print(slides[i].name);
            }
        }

        public void NextSlide(Vector2 pos = default)
        {
            if (!baseObject.activeSelf) return;
            print($"{currentSlide + 1}, {slides.Length}");

            if (currentSlide + 1 >= slides.Length)
            {
                Hide();
                return;
            }

            slides[currentSlide].SetActive(false);
            slides[++currentSlide].SetActive(true);
        }

        public void Show()
        {
            print("Showing");
            currentSlide = 0;
            baseObject.SetActive(true);
            slides[currentSlide].SetActive(true);
            Core.InputManager.Main.OnTapCompleted += NextSlide;
            TutorialActive = true;
        }

        public void Hide()
        {
            print("Hiding");
            seen = true;
            ActivatePanel.OnShow -= Show;
            baseObject.SetActive(false);
            slides[currentSlide].SetActive(false);
            Core.InputManager.Main.OnTapCompleted -= NextSlide;
            TutorialActive = false;
        }
    }
}
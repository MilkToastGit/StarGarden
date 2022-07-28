using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence : MonoBehaviour
{
    public bool Seen => seen;
    public Button ActivateButton;

    private GameObject baseObject;
    private GameObject[] slides;
    private int currentSlide;
    private bool seen = false;

    private void Awake()
    {
        ActivateButton?.onClick.AddListener(Show);

        baseObject = transform.GetChild(0).gameObject;

        slides = new GameObject[baseObject.transform.childCount];
        for (int i = 0; i < baseObject.transform.childCount; i++)
            slides[i] = baseObject.transform.GetChild(i).gameObject;
    }

    public void NextSlide()
    {
        if (!baseObject.activeSelf) return;

        if (currentSlide + 1 > slides.Length)
        {
            Hide();
            return;
        }

        slides[currentSlide].SetActive(false);
        slides[++currentSlide].SetActive(true);
    }

    public void Show()
    {
        currentSlide = 0;
        baseObject.SetActive(true);
    }

    public void Hide()
    {
        seen = true;
        ActivateButton.onClick.RemoveListener(Show);
        baseObject.SetActive(false);
    }
}

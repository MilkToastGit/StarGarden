using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [HideInInspector] public bool Completed = false;
        public delegate void ActivatedEvent(Tutorial tutorial);
        public event ActivatedEvent OnActivated;
        public delegate void CompletedEvent(Tutorial tutorial);
        public event CompletedEvent OnCompleted;
        
        [SerializeField] protected GameObject baseObject;
        [SerializeField] protected GameObject[] slides;
        protected int currentSlide;

        public virtual void NextSlide()
        {
            if (!baseObject.activeSelf) return;

            if (currentSlide + 1 >= slides.Length)
                return;

            slides[currentSlide].SetActive(false);
            slides[++currentSlide].SetActive(true);
        }
        
        public virtual void Show()
        {
            currentSlide = 0;
            baseObject.SetActive(true);
            slides[currentSlide].SetActive(true);
            OnActivated?.Invoke(this);
        }

        public virtual void Hide()
        {
            baseObject.SetActive(false);
            slides[currentSlide].SetActive(false);

            Completed = true;
            OnCompleted?.Invoke(this);
        }
    }
}
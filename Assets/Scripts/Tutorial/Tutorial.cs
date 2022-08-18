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
        public delegate void LastSlideReachedEvent(Tutorial tutorial);
        public event LastSlideReachedEvent OnLastSlideReached;
        
        [SerializeField] protected GameObject baseObject;
        [SerializeField] protected Slide[] slides;
        protected int currentSlide;

        public virtual void NextSlide(Vector2 pos = default)
        {
            if (!baseObject.activeSelf) return;
            print(currentSlide);

            if (currentSlide + 1 == slides.Length - 1)
            {
                OnLastSlideReached?.Invoke(this);
                Core.InputManager.Main.OnTapCompleted -= NextSlide;
            }
            else if (currentSlide + 1 >= slides.Length)
                return;

            slides[currentSlide].Hide();
            slides[++currentSlide].Show();
        }
        
        public virtual void Show()
        {
            currentSlide = 0;
            baseObject.SetActive(true);
            slides[currentSlide].Show();
            OnActivated?.Invoke(this);
            if (slides.Length == 1)
                OnLastSlideReached?.Invoke(this);

            Core.InputManager.Main.OnTapCompleted += NextSlide;
        }  

        public virtual void Hide()
        {
            baseObject.SetActive(false);
            slides[currentSlide].Hide();

            Completed = true;
        }
    }

    [System.Serializable]
    public class Slide
    {
        public GameObject Object;
        public UnityEngine.Events.UnityEvent Events;

        public void Show()
        {
            Object.SetActive(true);
            Events?.Invoke();
        }
        public void Hide() => Object.SetActive(false);
    }
}
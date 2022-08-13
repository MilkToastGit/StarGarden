using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace StarGarden.Core
{
    public class InteractionManager : MonoBehaviour, Manager
    {
        public static InteractionManager Main { get; private set; }

        public delegate void PassthroughEvent();
        public event PassthroughEvent OnPassthrough;

        List<Interactable> held = new List<Interactable>();

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            OnEnable();
        }

        public void LateInitialise() { }

        private void OnStartTouch(Vector2 pos)
        {
            if (UI.UIManager.Main.PanelShowing) return;

            Collider2D[] hits = Physics2D.OverlapPointAll(InputManager.Main.WorldTouchPosition);
            List<Interactable> interactables = new List<Interactable>();

            foreach (Collider2D hit in hits)
                if (hit.TryGetComponent(out Interactable t))
                    interactables.Add(t);

            interactables.Sort(new InteractableComparer());

            foreach (Interactable t in interactables)
            {
                held.Add(t);
                t.OnStartTouch();
                if (!t.Passthrough)
                    return;
            }

            OnPassthrough?.Invoke();
        }

        private void OnEndTouch(Vector2 pos)
        {
            foreach (Interactable t in held)
                t.OnEndTouch();
            held.Clear();
        }

        private void OnTap(Vector2 pos)
        {
            if (UI.UIManager.Main.PanelShowing) return;
            foreach (Collider2D collider in Physics2D.OverlapPointAll(InputManager.Main.WorldTouchPosition))
            {
                if (collider.TryGetComponent(out Interactable t))
                {
                    held.Add(t);
                    t.OnTap();
                    if (!t.Passthrough)
                        return;
                }
            }
        }

        private void OnEnable()
        {
            if (!InputManager.Main) return;
            InputManager.Main.OnTouchDown += OnStartTouch;
            InputManager.Main.OnTouchUp += OnEndTouch;
            InputManager.Main.OnTapCompleted += OnTap;
        }

        private void OnDisable()
        {
            if (!InputManager.Main) return;
            InputManager.Main.OnTouchDown -= OnStartTouch;
            InputManager.Main.OnTouchUp -= OnEndTouch;
            InputManager.Main.OnTapCompleted += OnTap;
        }
    }

    class InteractableComparer : IComparer<Interactable>
    {
        int IComparer<Interactable>.Compare(Interactable x, Interactable y)
        {
            return Comparer.Default.Compare(x.Layer, y.Layer);
        }
    }
}
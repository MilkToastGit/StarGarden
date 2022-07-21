using System.Collections.Generic;
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

        private void OnStartTouch()
        {
            if (UI.UIManager.Main.PanelShowing) return;
            foreach (Collider2D collider in Physics2D.OverlapPointAll(InputManager.Main.WorldTouchPosition))
            {
                if (collider.TryGetComponent(out Interactable t))
                {
                    held.Add(t);
                    t.OnStartTouch();
                    if (!t.Passthrough)
                        return;
                }
            }

            OnPassthrough?.Invoke();
        }

        private void OnEndTouch()
        {
            foreach (Interactable t in held)
                t.OnEndTouch();
            held.Clear();
        }

        private void OnTap()
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
            InputManager.Main.OnTouchDown += pos => OnStartTouch();
            InputManager.Main.OnTouchUp += pos => OnEndTouch();
            InputManager.Main.OnTapCompleted += pos => OnTap();
        }

        private void OnDisable()
        {
            if (!InputManager.Main) return;
            InputManager.Main.OnTouchDown -= pos => OnStartTouch();
            InputManager.Main.OnTouchUp -= pos => OnEndTouch();
            InputManager.Main.OnTapCompleted += pos => OnTap();
        }
    }
}
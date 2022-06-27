using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class InteractionManager : MonoBehaviour
    {
        public static InteractionManager Main { get; private set; }

        public delegate void PassthroughEvent();
        public event PassthroughEvent OnPassthrough;

        List<Interactable> held = new List<Interactable>();

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void OnStartTouch(Vector2 touchPosition)
        {
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

            print("passthrough");
            OnPassthrough?.Invoke();
        }

        private void OnEndTouch()
        {
            foreach (Interactable t in held)
                t.OnEndTouch();
            held.Clear();
        }

        private void OnEnable()
        {
            InputManager.Main.OnTouchDown += pos => OnStartTouch(pos);
            InputManager.Main.OnTouchUp += pos => OnEndTouch();
        }

        private void OnDisable()
        {
            InputManager.Main.OnTouchDown -= pos => OnStartTouch(pos);
            InputManager.Main.OnTouchUp -= pos => OnEndTouch();
        }
    }
}
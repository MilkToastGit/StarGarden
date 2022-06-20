using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    List<Interactable> held = new List<Interactable>();

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

using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    List<Touchable> currentlyTouched = new List<Touchable>();

    private void OnStartTouch(Vector2 touchPosition)
    {
        foreach (Collider2D collider in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(touchPosition)))
        {
            if (collider.TryGetComponent(out Touchable t))
            {
                currentlyTouched.Add(t);
                t.OnStartTouch();
                if (!t.Passthrough)
                    return;
            }
        }
    }

    private void OnEndTouch()
    {
        foreach (Touchable t in currentlyTouched)
            t.OnEndTouch();
        currentlyTouched.Clear();
    }

    private void OnEnable()
    {
        InputManager.Main.OnStartTouch += pos => OnStartTouch(pos);
        InputManager.Main.OnEndTouch += pos => OnEndTouch();
    }

    private void OnDisable()
    {
        InputManager.Main.OnStartTouch -= pos => OnStartTouch(pos);
        InputManager.Main.OnEndTouch -= pos => OnEndTouch();
    }
}

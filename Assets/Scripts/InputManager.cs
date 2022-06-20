using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Main;

    public Vector2 TouchPosition => touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    public Vector2 WorldTouchPosition => Camera.main.ScreenToWorldPoint(TouchPosition);

    public delegate void TouchDownEvent(Vector2 position);
    public event TouchDownEvent OnTouchDown;
    public delegate void TouchUpEvent(Vector2 position);
    public event TouchUpEvent OnTouchUp;
    public delegate void TouchHoldEvent(Vector2 position);
    public event TouchHoldEvent OnTouchHold;

    private TouchControls touchControls;

    private Vector2 holdStartPosition;
    private bool awaitingStartTouch;

    private void Update()
    {
        if (awaitingStartTouch)
        {
            OnTouchDown?.Invoke(TouchPosition);
            holdStartPosition = TouchPosition;
            awaitingStartTouch = false;
        }
    }

    private void Awake()
    {
        if (!Main)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject); touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => awaitingStartTouch = true;
        touchControls.Touch.TouchPress.canceled += ctx => OnTouchUp?.Invoke(TouchPosition);
        touchControls.Touch.TouchHold.performed += ctx => HoldPerformed(ctx);
    }

    private void HoldPerformed(InputAction.CallbackContext ctx)
    {
        if (F.FastDistance(holdStartPosition, TouchPosition) < 1000)
            OnTouchHold?.Invoke(TouchPosition);
    }
}

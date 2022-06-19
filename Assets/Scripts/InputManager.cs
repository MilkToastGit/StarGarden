using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Main;

    public Vector2 TouchPosition => touchControls.Touch.TouchPosition.ReadValue<Vector2>();

    public delegate void StartTouchEvent(Vector2 position);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position);
    public event EndTouchEvent OnEndTouch;
    public delegate void HoldTouchEvent(Vector2 position);
    public event HoldTouchEvent OnHoldTouch;

    private TouchControls touchControls;

    private Vector2 holdStartPosition;

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
        touchControls.Touch.TouchPress.started += ctx => OnStartTouch?.Invoke(TouchPosition);
        touchControls.Touch.TouchPress.canceled += ctx => OnEndTouch?.Invoke(TouchPosition);
        touchControls.Touch.TouchHold.started += ctx => HoldStarted(ctx);
        touchControls.Touch.TouchHold.performed += ctx => HoldPerformed(ctx);
    }

    private void HoldStarted(InputAction.CallbackContext ctx)
    {
        holdStartPosition = TouchPosition;
    }

    private void HoldPerformed(InputAction.CallbackContext ctx)
    {
        if (F.FastDistance(holdStartPosition, TouchPosition) < 1000)
            OnHoldTouch?.Invoke(TouchPosition);
    }
}

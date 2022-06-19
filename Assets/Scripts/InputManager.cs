using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Main;

    public Vector2 TouchPosition => touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    public Vector2 WorldTouchPosition => Camera.main.ScreenToWorldPoint(TouchPosition);

    public delegate void StartTouchEvent(Vector2 position);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position);
    public event EndTouchEvent OnEndTouch;
    public delegate void HoldTouchEvent(Vector2 position);
    public event HoldTouchEvent OnHoldTouch;

    private TouchControls touchControls;

    private Vector2 holdStartPosition;
    private bool awaitingStartTouch;

    private void Update()
    {
        if (awaitingStartTouch)
        {
            OnStartTouch?.Invoke(TouchPosition);
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
        touchControls.Touch.TouchPress.canceled += ctx => OnEndTouch?.Invoke(TouchPosition);
        touchControls.Touch.TouchHold.performed += ctx => HoldPerformed(ctx);
    }

    private void HoldPerformed(InputAction.CallbackContext ctx)
    {
        if (F.FastDistance(holdStartPosition, TouchPosition) < 1000)
            OnHoldTouch?.Invoke(TouchPosition);
    }
}

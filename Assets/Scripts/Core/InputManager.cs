using UnityEngine;
using UnityEngine.InputSystem;

namespace StarGarden.Core
{
    public class InputManager : MonoBehaviour, Manager
    {
        public static InputManager Main { get; private set; }

        public Vector2 TouchPosition => touchControls.Touch.TouchPosition.ReadValue<Vector2>();
        public Vector2 WorldTouchPosition => Camera.main.ScreenToWorldPoint(TouchPosition);

        public delegate void TouchDownEvent(Vector2 position);
        public event TouchDownEvent OnTouchDown;
        public delegate void TouchUpEvent(Vector2 position);
        public event TouchUpEvent OnTouchUp;
        public delegate void TapCompletedEvent(Vector2 position);
        public event TapCompletedEvent OnTapCompleted;
        public delegate void TouchDragEvent(Vector2 position);
        public event TouchDragEvent OnTouchDrag;
        public delegate void TouchHoldEvent(Vector2 position);
        public event TouchHoldEvent OnTouchHold;

        private TouchControls touchControls;

        private Vector2 holdStartPosition;
        private bool awaitingStartTouch;
        private bool awaitingDrag;

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject); 
            touchControls = new TouchControls();
            touchControls.Enable();
        }

        public void LateInitialise() { }

        private void Update()
        {
            if (awaitingStartTouch)
            {
                OnTouchDown?.Invoke(TouchPosition);
                holdStartPosition = TouchPosition;
                awaitingStartTouch = false;
                awaitingDrag = true;
            }
            if (awaitingDrag && F.FastDistance(holdStartPosition, TouchPosition) >= 1000)
            {
                OnTouchDrag?.Invoke(TouchPosition);
                awaitingDrag = false;
            }
        }

        private void OnEnable()
        {
            if(touchControls != null)
                touchControls.Enable();
        }

        private void OnDisable()
        {
            if(touchControls != null)
                touchControls.Disable();
        }

        private void Start()
        {
            touchControls.Touch.TouchPress.started += ctx => awaitingStartTouch = true;
            touchControls.Touch.TouchPress.canceled += ctx => { OnTouchUp?.Invoke(TouchPosition); awaitingDrag = false; };
            touchControls.Touch.TouchTap.performed += ctx => { OnTapCompleted?.Invoke(TouchPosition); };
            touchControls.Touch.TouchHold.performed += ctx => HoldPerformed(ctx);
        }

        private void PressCanceled(InputAction.CallbackContext ctx)
        {

        }

        private void HoldPerformed(InputAction.CallbackContext ctx)
        {
            if (F.FastDistance(holdStartPosition, TouchPosition) < 1000)
            {
                OnTouchHold?.Invoke(TouchPosition);
                awaitingDrag = false;
            }
        }
    }

}
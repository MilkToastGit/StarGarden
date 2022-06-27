using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class CameraControl : MonoBehaviour
    {
        public float dragMultiplier = 1, slideFactor;

        private int currentIsland;
        private bool dragging = false;
        private float startTouchX;
        private float startCamX;

        private float camWidth;
        private float xVelocity;

        private void Awake()
        {
            camWidth = GetComponent<Camera>().GetViewportRect().width;
        }

        private void SetPosition(float xPos)
        {
            Rect bounds = IslandManager.Main.Islands[currentIsland].Bounds;

            float xMin = bounds.xMin + camWidth / 2;
            float xMax = bounds.xMax - camWidth / 2;

            float newPosX = Mathf.Clamp(xPos, xMin, xMax);

            xVelocity = newPosX - transform.position.x;
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        }

        private void Update()
        {
            if (!dragging)
            {
                xVelocity *= slideFactor;
                print(xVelocity);
                SetPosition(transform.position.x + xVelocity);
                return;
            }

            float offset = startTouchX - InputManager.Main.TouchPosition.x;
            SetPosition(startCamX + (offset / Screen.width * camWidth) * dragMultiplier);
        }

        private void OnTouchDrag(Vector2 position)
        {
            startTouchX = position.x;
            startCamX = transform.position.x;
            dragging = true;
        }
        
        private void OnTouchUp(Vector2 position)
        {
            dragging = false;
        }

        private void OnEnable()
        {
            InputManager.Main.OnTouchDrag += OnTouchDrag;
            InputManager.Main.OnTouchUp += OnTouchUp;
        }

        private void OnDisable()
        {
            InputManager.Main.OnTouchDrag -= OnTouchDrag;
            InputManager.Main.OnTouchUp -= OnTouchUp;
        }
    }
}
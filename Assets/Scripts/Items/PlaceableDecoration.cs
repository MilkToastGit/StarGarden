using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Items
{
    public class PlaceableDecoration : MonoBehaviour, Interactable
    {
        public static PlaceableDecoration currentlyMoving;

        public bool Passthrough => false;

        private DecorationInstances decor;
        private State state = State.Idle;
        private bool firstPlacement, hoveringInventory, invalidSpace, tapPerformedLastFrame;
        private SpriteRenderer sprite;

        private Vector2Int lastPlacedPosition;

        public void SetItem(DecorationInstances decor, bool firstPlacement)
        {
            this.decor = decor;
            sprite = Instantiate(decor.Item.Prefab, transform.GetChild(0)).GetComponentInChildren<SpriteRenderer>();
            if (firstPlacement)
            {
                state = State.AwaitingDrag;
                this.firstPlacement = firstPlacement;
            }
        }

        private void Place(Vector2Int position)
        {
            if (firstPlacement)
            {
                decor.Place(position);
                firstPlacement = false;
            }
            else
                decor.Move(lastPlacedPosition, position);

            lastPlacedPosition = position;
            currentlyMoving = null;
            state = State.Idle;
            print(decor.InventoryCount);
        }

        private void ReturnToInventory()
        {
            if (!firstPlacement)
                decor.Unplace(lastPlacedPosition);
            print("destroying");
            Destroy(gameObject);
        }

        private void CancelDrag()
        {
            if (firstPlacement)
                ReturnToInventory();
            else
            {
                transform.position = WorldGrid.GridToWorld(lastPlacedPosition);
                sprite.color = Color.white;
                state = State.Idle;
            }
        }

        private void StartDragging()
        {
            state = State.Dragging;
            currentlyMoving = this;
        }

        private void Update()
        {
            if (tapPerformedLastFrame && state == State.AwaitingDrag)
                CancelDrag();
            else
                tapPerformedLastFrame = false;

            if (state != State.Dragging) return;

            Vector2 touchPos = InputManager.Main.TouchPosition / Screen.height;
            hoveringInventory = touchPos.x < 0.15f && touchPos.y > 0.85f;

            if (hoveringInventory)
                transform.position = InputManager.Main.WorldTouchPosition;
            else
            {
                Vector2Int gridPos = WorldGrid.WorldToGrid(InputManager.Main.WorldTouchPosition);
                transform.position = WorldGrid.GridToWorld(gridPos);
                if (gridPos == lastPlacedPosition)
                    invalidSpace = false;
                else
                    invalidSpace = CheckInvalidSpace(gridPos);

                sprite.color = invalidSpace ? Color.red : Color.white;
            }
        }

        private bool CheckInvalidSpace(Vector2Int position)
        {
            bool withinIsland = false;
            Vector2 worldPosition = WorldGrid.GridToWorld(position);
            foreach (Island island in IslandManager.Main.Islands)
            {
                if (F.WithinBounds(worldPosition, island.Bounds))
                {
                    withinIsland = true;
                    continue;
                }
            }
            if (!withinIsland) return true;

            foreach (DecorationInstances decor in InventoryManager.Main.GetAllItemsFromCategory(0))
                if (decor.IsPositionTaken(position)) return true;

            return false;
        }

        public void OnStartTouch()
        {
            if (state == State.AwaitingDrag)
                StartDragging();
            else
                state = State.Touched;
        }

        public void OnEndTouch()
        {
            if (state == State.Dragging)
            {
                if (hoveringInventory)
                    ReturnToInventory();
                else if (invalidSpace)
                    state = State.AwaitingDrag;
                else
                    Place(WorldGrid.WorldToGrid(InputManager.Main.WorldTouchPosition));
            }
            else if (state == State.Touched)
                state = State.Idle;
        }

        private void OnHoldTouch()
        {
            if (state == State.Touched)
                StartDragging();
        }

        private void OnEnable()
        {
            InputManager.Main.OnTouchHold += pos => OnHoldTouch();
            InputManager.Main.OnTouchDown += pos => tapPerformedLastFrame = true;
        }

        private void OnDisable()
        {
            InputManager.Main.OnTouchDown -= pos => OnHoldTouch();
            InputManager.Main.OnTouchDown -= pos => tapPerformedLastFrame = true;
        }

        public enum State
        {
            Idle,
            Touched,
            Dragging,
            AwaitingDrag
        }
    }
}
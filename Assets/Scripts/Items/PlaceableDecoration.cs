using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Items
{
    public class PlaceableDecoration : MonoBehaviour, Interactable
    {
        public static bool placingDecoration = false;

        public bool Passthrough => false;

        private DecorationInstances decorInst;
        private Decoration decor => decorInst.Item as Decoration;
        private State state = State.Idle;
        private bool firstPlacement, hoveringInventory, invalidSpace, tapPerformedLastFrame;
        private SpriteRenderer sprite;
        private Vector2 centerOffset;

        private Vector2Int lastPlacedPoint, lastHeldPoint;

        public void SetItem(DecorationInstances decoration, bool firstPlacement)
        {
            decorInst = decoration;
            centerOffset = new Vector2(
                (decor.Size.x - 1) % 2 / 2f,
                (decor.Size.y - 1) % 2 / 2f) * WorldGrid.Spacing;

            sprite = Instantiate(decoration.Item.Prefab, transform.GetChild(0)).GetComponentInChildren<SpriteRenderer>();
            if (firstPlacement)
            {
                state = State.AwaitingDrag;
                this.firstPlacement = firstPlacement;
            }
        }

        private void Place(Vector2Int point)
        {
            if (firstPlacement)
            {
                decorInst.Place(point);
                firstPlacement = false;
            }
            else
                decorInst.Move(lastPlacedPoint, point);

            lastPlacedPoint = point;
            placingDecoration = false;
            state = State.Idle;
            print(decorInst.InventoryCount);
        }

        private void ReturnToInventory()
        {
            if (!firstPlacement)
                decorInst.Unplace(lastPlacedPoint);
            placingDecoration = false;
            Destroy(gameObject);
        }

        private void CancelDrag()
        {
            if (firstPlacement)
                ReturnToInventory();
            else
            {
                transform.position = WorldGrid.GridToWorld(lastPlacedPoint) + centerOffset;
                sprite.color = Color.white;
                state = State.Idle;
            }
            placingDecoration = false;
        }

        private void StartDragging()
        {
            state = State.Dragging;
            placingDecoration = true;
        }

        private void Update()
        {
            foreach (Vector2Int point in decorInst.GetGridPoints(WorldGrid.WorldToGrid((Vector2)transform.position - centerOffset)))
                Debug.DrawRay(WorldGrid.GridToWorld(point), Vector2.up * 0.25f, Color.green);

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

                if (gridPos == lastHeldPoint) return;
                else lastHeldPoint = gridPos;

                transform.position = WorldGrid.GridToWorld(gridPos) + centerOffset;
                if (gridPos == lastPlacedPoint)
                    invalidSpace = false;
                else
                    invalidSpace = CheckInvalidSpace(gridPos);

                sprite.color = invalidSpace ? Color.red : Color.white;
            }
        }

        private bool CheckInvalidSpace(Vector2Int point)
        {
            decorInst.GetGridRange(point, out Vector2Int min, out Vector2Int max);

            // If min or max points outside of islands
            if (IslandManager.Main.WithinIsland(WorldGrid.GridToWorld(min)) < 0 ||
                IslandManager.Main.WithinIsland(WorldGrid.GridToWorld(max)) < 0)
                return true;

            foreach (DecorationInstances decor in InventoryManager.Main.GetAllItemsFromCategory(0))
                if (decor.DoDecorOverlap(min, max, lastPlacedPoint, !firstPlacement))
                    return true;

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
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
        public int Layer => (int)InteractableLayer.Decoration;

        [SerializeField] private GameObject smokePuff;
        private DecorationInstances decorInst;
        private Decoration decor => decorInst.Item as Decoration;
        private State state = State.Idle;
        private bool firstPlacement, hoveringInventory, invalidSpace, tapPerformedLastFrame;
        private SpriteRenderer sprite;
        private Vector2 centerOffset;
        
        private Vector2Int lastPlacedPoint, lastHeldPoint;

        public void SetItem(DecorationInstances decoration, bool place, Vector3Int point = default)
        {
            decorInst = decoration;
            centerOffset = new Vector2(
                (decor.Size.x - 1) % 2 / 2f,
                (decor.Size.y - 1) % 2 / 2f) * WorldGrid.Spacing;
            GetComponent<CircleCollider2D>().radius = Mathf.Max(Mathf.Max(decor.Size.x * WorldGrid.Spacing.x, decor.Size.y * WorldGrid.Spacing.y) / 2f, 0.5f);

            sprite = Instantiate(decoration.Item.Prefab, transform.GetChild(0)).GetComponentInChildren<SpriteRenderer>();
            if (place)
            {
                lastPlacedPoint = (Vector2Int)point;
                placingDecoration = false;
                state = State.Idle;
                firstPlacement = false;
                transform.SetParent(IslandManager.Main.Islands[point.z].IslandObject.transform);
            }
            else
            {
                state = State.AwaitingDrag;
                firstPlacement = true;
                transform.SetParent(IslandManager.Main.ActiveIsland.IslandObject.transform);
            }
        }

        private void Place(Vector2Int point)
        {
            if (firstPlacement)
            {
                decorInst.Place(point, IslandManager.Main.ActiveIsland.Index);
                firstPlacement = false;
            }
            else
                decorInst.Move(lastPlacedPoint, point, IslandManager.Main.ActiveIsland.Index);

            lastPlacedPoint = point;
            placingDecoration = false;
            state = State.Idle;
            UI.InventorySack.SetState(false, false);
            sprite.sortingLayerName = "Default";
            CreateSmokePuff();
            //print(decorInst.InventoryCount);
        }

        private void ReturnToInventory()
        {
            if (!firstPlacement)
                decorInst.Unplace(lastPlacedPoint, IslandManager.Main.ActiveIsland.Index);
            placingDecoration = false;
            UI.InventorySack.SetState(false, false);
            Destroy(gameObject);
        }

        private void Mistap()
        {
            if (firstPlacement)
                StartDragging();
            else
            {
                transform.position = WorldGrid.GridToWorld(lastPlacedPoint) + centerOffset;
                sprite.color = Color.white;
                state = State.Idle;
                sprite.sortingLayerName = "Default";
            }
            placingDecoration = false;
            UI.InventorySack.SetState(false, false);
        }

        private void StartDragging()
        {
            state = State.Dragging;
            placingDecoration = true;
            UI.InventorySack.SetState(true, false);
            sprite.sortingLayerName = "Effects";
        }

        private void Update()
        {
            foreach (Vector2Int point in decorInst.GetGridPoints(WorldGrid.WorldToGrid((Vector2)transform.position - centerOffset)))
                Debug.DrawRay(WorldGrid.GridToWorld(point), Vector2.up * 0.25f, Color.green);

            if (tapPerformedLastFrame && state == State.AwaitingDrag)
                Mistap();
            else
                tapPerformedLastFrame = false;

            if (state != State.Dragging) return;

            Vector2 touchPos = InputManager.Main.TouchPosition / new Vector2 (Screen.width, Screen.height);
            hoveringInventory = touchPos.x > 0.75f && touchPos.y > 0.85f;

            if (hoveringInventory)
            {
                transform.position = InputManager.Main.WorldTouchPosition;
                UI.InventorySack.SetState(true, true);
            }
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
                UI.InventorySack.SetState(true, false);
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
                if (decor.DoDecorOverlap(min, max, lastPlacedPoint, IslandManager.Main.ActiveIsland.Index, !firstPlacement))
                    return true;

            return false;
        }

        private void CreateSmokePuff()
        {
            smokePuff.SetActive(false);
            smokePuff.transform.position = sprite.transform.position;
            smokePuff.SetActive(true);
            Invoke("DisableSmokePuff", 1f);
        }
        private void DisableSmokePuff() => smokePuff.SetActive(false);

        public void OnStartTouch()
        {
            if (state == State.AwaitingDrag)
                StartDragging();
            else
                state = State.Touched;
        }

        public void OnTouchUp(Vector2 pos)
        {
            if (state == State.Dragging)
            {
                print($"state: {state}, hovering: {hoveringInventory}, invalid: {invalidSpace}");
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

        private void OnHoldTouch(Vector2 pos)
        {
            if (state == State.Touched)
                StartDragging();
        }

        public void OnEndTouch() { }

        public void OnTap() { }

        private void SetTapPerformed(Vector2 pos) => tapPerformedLastFrame = true;

        private void OnEnable()
        {
            InputManager.Main.OnTouchHold += OnHoldTouch;
            InputManager.Main.OnTouchDown += SetTapPerformed;
            InputManager.Main.OnTouchUp += OnTouchUp;
        }

        private void OnDisable()
        {
            InputManager.Main.OnTouchHold -= OnHoldTouch;
            InputManager.Main.OnTouchDown -= SetTapPerformed;
            InputManager.Main.OnTouchUp -= OnTouchUp;
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
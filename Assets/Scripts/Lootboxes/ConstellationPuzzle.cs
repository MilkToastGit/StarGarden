using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.LootBoxes
{
    public class ConstellationPuzzle : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float rotateSensitivity;
        [Range(0f, 1f)]
        public float inertia;
        public float angleThreshold;
        public float checkDuration;
        public GameObject LinePrefab;
        public ConstellationPoint[] ConstellationPoints;

        private bool dragging = false;
        private bool atCorrectAngle = false;
        private float lastXDelta = 0f;
        private float xDelta = 0f;
        private Vector2 lastTouchPos;

        private void Awake()
        {
            SpawnConstellation();
            transform.Rotate(Vector3.up, Random.Range(30f, 180f) * (Random.value > 0.5f ? 1 : -1));
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (!dragging)
            {
                xDelta *= inertia;
                CheckCorrect();
            }
            else
            {
                if (Mathf.Abs(xDelta) > 0f)
                    lastXDelta = xDelta;
                xDelta = InputManager.Main.TouchPosition.x - lastTouchPos.x;
                lastTouchPos = InputManager.Main.TouchPosition;
            }

            transform.Rotate(Vector3.up, -xDelta * rotateSensitivity, Space.World);
        }

        private void CheckCorrect()
        {
            atCorrectAngle = !transform.rotation.eulerAngles.y.Between(angleThreshold / 2, 360 - angleThreshold / 2);
            if (atCorrectAngle && xDelta < 0.001f)
            {
                xDelta = 0f;
                transform.rotation = Quaternion.identity;
                print("You Di d i  t.");
            }
        }

        private void SpawnConstellation()
        {
            List<ConstellationPoint> nextPoints = new List<ConstellationPoint>() { ConstellationPoints[0] };
            while (nextPoints.Count > 0)
            {
                ConstellationPoint[] lastPoints = nextPoints.ToArray();
                nextPoints.Clear();

                foreach(ConstellationPoint lastPoint in lastPoints)
                {
                    foreach (int nextPoint in lastPoint.NextPoints)
                    {
                        SpawnLine(lastPoint.Position3D, ConstellationPoints[nextPoint].Position3D);
                        nextPoints.Add(ConstellationPoints[nextPoint]);
                    }
                }
            }
        }

        private void SpawnLine(Vector3 start, Vector3 end)
        {
            Vector3[] positions = new Vector3[] { start, end };
            Instantiate(LinePrefab, transform).GetComponent<LineRenderer>().SetPositions(positions);
        }

        private void OnDrawGizmos()
        {
            foreach (ConstellationPoint point in ConstellationPoints)
                Gizmos.DrawWireSphere(point.Position, 0.2f);
        }

        private void OnTouchDrag(Vector2 position)
        {
            if (Items.PlaceableDecoration.placingDecoration) return;

            lastTouchPos = position;
            dragging = true;
        }

        private void OnTouchUp(Vector2 position)
        {
            xDelta = lastXDelta;
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

    [System.Serializable]
    public class ConstellationPoint
    {
        public Vector2 Position;
        public int[] NextPoints;
        public Vector3 Position3D { 
            get {
                if (!zPosSet)
                {
                    zPosition = Random.Range(-2f, 2f);
                    zPosSet = true;
                }
                return new Vector3(Position.x, Position.y, zPosition);
            }
        }

        private float zPosition;
        private bool zPosSet = false;
    }
}
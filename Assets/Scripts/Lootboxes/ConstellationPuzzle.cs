using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.LootBoxes
{
    public class ConstellationPuzzle : MonoBehaviour
    {
        public int previewConstellation = 0;
        [Range(0f, 1f)]
        public float rotateSensitivity;
        [Range(0f, 1f)]
        public float inertia;
        public float angleThreshold;
        public float checkDuration;
        public float introDuration;
        public AnimationCurve spinIntro;
        public GameObject linePrefab, starPrefab, starGlowPrefab;
        public Constellation[] Constellations;
        public delegate void PuzzleCompleted();

        private float introTime = 0f;
        private float introRotationTarget;
        private bool dragging = false;
        private bool atCorrectAngle = false;
        private bool puzzleComplete = false;
        private float lastXDelta = 0f;
        private float xDelta = 0f;
        private Vector2 lastTouchPos;
        private Transform starGlowParent;
        private PuzzleCompleted onPuzzleCompleted;

        private void Awake()
        {
            starGlowParent = new GameObject().transform;
            starGlowParent.SetParent(transform.parent);
            starGlowParent.localPosition = Vector3.zero;
            //SetPuzzle(()=>print("done"));
        }

        public void SetPuzzle(PuzzleCompleted puzzleCompleted)
        {
            onPuzzleCompleted = puzzleCompleted;

            transform.rotation = Quaternion.identity;
            transform.Order66();
            starGlowParent.Order66();
            SpawnConstellation(Random.Range(0, Constellations.Length));

            introRotationTarget = Random.Range(90f, 170f) * (Random.value > 0.5f ? 1 : -1);

            introTime = 0f;
            lastXDelta = 0f;
            xDelta = 0f;
            lastTouchPos = Vector2.zero;
            puzzleComplete = false;
        }

        private void Update()
        {
            if (puzzleComplete) return;

            if (introTime < introDuration)
                Spin();
            else
                Rotate();
        }

        private void Spin()
        {
            float t = introTime / introDuration;
            t = spinIntro.Evaluate(t);
            float rotation = t * 360 * 3 + introRotationTarget;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
            introTime += Time.deltaTime;
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
                puzzleComplete = true;
                onPuzzleCompleted?.Invoke();
                print("You Di d i  t.");
            }
        }

        private void SpawnConstellation(int index)
        {
            //List<ConstellationPoint> nextPoints = new List<ConstellationPoint>() { constellation[0] };
            //while (nextPoints.Count > 0)
            //{
            //    ConstellationPoint[] lastPoints = nextPoints.ToArray();
            //    nextPoints.Clear();

            //    foreach(ConstellationPoint lastPoint in lastPoints)
            //    {
            //        foreach (int nextPoint in lastPoint.NextPoints)
            //        {
            //            SpawnLine(lastPoint.Position3D, constellation[nextPoint].Position3D);
            //            nextPoints.Add(constellation[nextPoint]);
            //        }
            //    }
            //}

            foreach (ConstellationPoint point in Constellations[index])
            {
                SpawnStar(point.Position3D, point.radius);

                foreach (int nextPoint in point.NextPoints)
                    SpawnLine(point.Position3D, Constellations[index][nextPoint].Position3D);
            }
        }

        private void SpawnLine(Vector3 start, Vector3 end)
        {
            Vector3[] positions = new Vector3[] { start, end };
            Instantiate(linePrefab, transform).GetComponent<LineRenderer>().SetPositions(positions);
        }

        private void SpawnStar(Vector3 position, float radius)
        {
            Transform star = Instantiate(starPrefab, transform).transform;
            star.localPosition = position;
            star.localScale = radius * Vector3.one;
            Transform starGlow = Instantiate(starGlowPrefab, starGlowParent).transform;
            starGlow.localPosition = (Vector2)position;
            starGlow.localScale = radius * Vector3.one;
        }

        private void OnDrawGizmos()
        {
            foreach (ConstellationPoint point in Constellations[previewConstellation])
                Gizmos.DrawWireSphere(point.Position, point.radius);
        }

        private void OnTouchDrag(Vector2 position)
        {
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
}
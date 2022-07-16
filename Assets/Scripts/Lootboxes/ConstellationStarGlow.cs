using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.LootBoxes
{
    public class ConstellationStarGlow : MonoBehaviour
    {
        public Vector2 SizeRange, AlphaRange;
        public AnimationCurve tCurve;

        private Transform constellation;
        private SpriteRenderer sprite;
        private float angleThreshold, startScale, fadeInTime, fadeInDuration;

        private void Awake()
        {
            ConstellationPuzzle puzzle = FindObjectOfType<ConstellationPuzzle>();

            constellation = puzzle.transform;
            angleThreshold = puzzle.angleThreshold;
            fadeInDuration = puzzle.introDuration;
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (startScale == 0f)
                startScale = transform.localScale.x;

            float t = Mathf.Abs(F.NormalizeAngle(constellation.localEulerAngles.y)).Map(180f, angleThreshold / 2f);
            t = tCurve.Evaluate(t);
            SetSpriteAlpha(Mathf.Lerp(AlphaRange.x, AlphaRange.y, t) * Mathf.Pow(fadeInTime / fadeInDuration, 2));
            SetSize(Mathf.Lerp(SizeRange.x, SizeRange.y, t));

            fadeInTime += Time.deltaTime;
        }

        private void SetSpriteAlpha(float alpha)
        {
            Color c = sprite.color;
            c.a = alpha;
            sprite.color = c;
        }

        private void SetSize(float mult)
        {
            transform.localScale = Vector3.one * startScale * mult;
        }
    }
}
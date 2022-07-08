using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Stardust
{
    public class Starfall : MonoBehaviour, Interactable
    {
        public bool Passthrough => false;

        [SerializeField] private Transform star;

        private Rarity rarity;
        private Vector2 spawnPos = new Vector2(30, 50);
        private CircleCollider2D circleCollider;
        private float landT = 0, bounceT = 0;
        private float bounceHeight;

        private void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;

            rarity = Random.value > 0.8f ? Rarity.Rare : Rarity.Common;
            if (rarity == Rarity.Rare) GetComponentInChildren<SpriteRenderer>().color = Color.cyan;

            bounceHeight = Random.Range(0.5f, 1.5f);
            star.position = spawnPos;
        }

        private void Update()
        {
            if (landT < 1)
            {
                landT = Mathf.Clamp01(landT + Time.deltaTime / 2);
                star.localPosition = Vector2.Lerp(spawnPos, Vector2.zero, landT);
            }
            else if (bounceHeight > 0)
            {
                bounceT = Mathf.Clamp01(bounceT + Time.deltaTime / (bounceHeight / 2 + 0.5f));
                star.localPosition = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, bounceT)) * Vector2.up * bounceHeight;
                if (bounceT >= 1)
                {
                    bounceHeight = Mathf.Max(bounceHeight / 3 - 0.1f, 0);
                    bounceT = 0;
                }
            }

            if (landT == 1 && !circleCollider.enabled)
                circleCollider.enabled = true;
        }

        public void OnTap()
        {
            ResourcesManager.Main.AddStardust(rarity, 1);
            Destroy(gameObject);
        }

        public void OnStartTouch() { }
        public void OnEndTouch() { }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Stardust
{
    public class Starfall : MonoBehaviour, Interactable
    {
        public static readonly Vector2 RareRateRange = new Vector2(0.1f, 0.45f);

        public delegate void CollectedEvent(Starfall starfall);
        public event CollectedEvent OnCollected;

        public bool Passthrough => false;
        public int Layer => (int)InteractableLayer.Starfall;
        public bool Collected => collected;

        [SerializeField] private Transform star;
        [SerializeField] private GameObject commonEffect, rareEffect;
        [SerializeField] private SpriteRenderer shadow;

        private Rarity rarity;
        private bool collected = false;
        private int island;

        public void Initialise(bool instant, bool trailOnly)
        {
            AutoCollection.OnAutoCollectionActivated += Collect;

            rarity = Random.value > Pets.PetManager.Main.CollectiveHappiness.Map(0f, 1f, RareRateRange.x, RareRateRange.y) ?
                Rarity.Common : Rarity.Rare;

            if (rarity == Rarity.Rare) GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0.6f, 1f);

            if (instant)
            {
                OnLand();
                return;
            }
            
            if (trailOnly)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                star.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(transform.parent.gameObject, 5f);
            }

            GetComponent<Animator>().SetTrigger("Enter");
        }

        public void OnTap()
        {
            Collect();
        }

        public void OnLand()
        {
            if (AutoCollection.Active)
                Collect();
        }

        public void Collect()
        {
            if (collected) return;

            collected = true;
            ResourcesManager.Main.AddStardust(rarity, 1);

            star.gameObject.SetActive(false);
            shadow.enabled = false;
            if (rarity == Rarity.Common) commonEffect.SetActive(true);
            else rareEffect.SetActive(true);

            OnCollected?.Invoke(this);
            Destroy(transform.parent.gameObject, 2f);
        }

        public void OnStartTouch() { }
        public void OnEndTouch() { }

        private void OnDestroy()
        {
            AutoCollection.OnAutoCollectionActivated -= Collect;
        }
    }
}
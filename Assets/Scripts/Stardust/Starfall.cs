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

        public void Initialise(bool instant, bool trailOnly)
        {
            rarity = Random.value > Pets.PetManager.Main.CollectiveHappiness.Map(0f, 1f, 0.1f, 0.6f) ?
                Rarity.Common : Rarity.Rare;
            if (rarity == Rarity.Rare) GetComponentInChildren<SpriteRenderer>().color = Color.cyan;

            if(!instant)
                GetComponent<Animator>().SetTrigger("Enter");
            if (trailOnly)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                star.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(transform.parent.gameObject, 5f);
            }
        }

        public void OnTap()
        {
            ResourcesManager.Main.AddStardust(rarity, 1);
            Destroy(transform.parent.gameObject);
        }

        public void OnStartTouch() { }
        public void OnEndTouch() { }
    }
}
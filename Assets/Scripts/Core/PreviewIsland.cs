using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class PreviewIsland : MonoBehaviour, Interactable
    {
        public IslandNavigation navigation;
        public int Layer => (int)InteractableLayer.Default;

        public int island;

        public bool Passthrough => false;

        [SerializeField] private SpriteRenderer starGlow;

        public void OnTap() => navigation.ZoomIntoIsland(island);

        public void OnEndTouch() { }

        public void OnStartTouch() { }

        public void UpdateStarGlow()
        {
            if (Stardust.AutoCollection.Active) return;

            foreach (Stardust.Starfall s in IslandManager.Main.Islands[island].IslandObject.GetComponentsInChildren<Stardust.Starfall>(true))
            {
                if (!s.Collected)
                {
                    starGlow.enabled = true;
                    return;
                }
            }

            starGlow.enabled = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class IslandSelection : MonoBehaviour, Interactable
    {
        public IslandNavigation navigation;
        public int Layer => (int)InteractableLayer.Default;

        public int island;

        public bool Passthrough => false;

        public void OnTap() => navigation.ZoomIntoIsland(island);

        public void OnEndTouch() { }

        public void OnStartTouch() { }
    }
}
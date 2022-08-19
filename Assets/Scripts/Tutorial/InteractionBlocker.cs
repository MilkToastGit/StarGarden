using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Tutorial
{
    public class InteractionBlocker : MonoBehaviour, Interactable
    {
        public bool Passthrough => false;

        public int Layer => 10;

        public void OnEndTouch() { }
        public void OnStartTouch() { }
        public void OnTap() { print("Tapped"); }
    }
}
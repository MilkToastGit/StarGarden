using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public interface Interactable
    {
        public bool Passthrough { get; }
        public void OnStartTouch();
        public void OnEndTouch();
    }
}
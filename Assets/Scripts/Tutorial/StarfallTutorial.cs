using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Stardust;

namespace StarGarden.Tutorial
{
    public class StarfallTutorial : Tutorial
    {
        public CameraControl cameraControl;
        public GameObject starfall;

        public override void Show()
        {
            base.Show();
            Starfall s = Instantiate(starfall, IslandManager.Main.ActiveIsland.IslandObject.transform).GetComponent<Starfall>();
            s.OnLanded += NextSlide;
            s.OnCollected += OnStarfallCollected;
            cameraControl.enabled = false;
        }

        private void OnStarfallCollected(Starfall starfall)
        {
            NextSlide();
            starfall.OnLanded -= NextSlide;
            starfall.OnCollected -= OnStarfallCollected;
            InputManager.Main.OnTapCompleted += OnTap;
        }

        private void OnTap(Vector2 pos)
        {
            NextSlide();
        }

        public override void Hide()
        {
            base.Hide();
            cameraControl.enabled = true;
            InputManager.Main.OnTapCompleted -= OnTap;
        }
    }
}


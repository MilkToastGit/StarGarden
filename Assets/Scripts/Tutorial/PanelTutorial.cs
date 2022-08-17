using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class PanelTutorial : Tutorial.Tutorial
    {
        public UIPanel ActivatePanel;

        private void Awake()
        {
            if (Completed) return;
            ActivatePanel.OnShow += Show;
        }

        private void NextSlide(Vector2 pos) => NextSlide();

        public override void Show()
        {
            base.Show();
            Core.InputManager.Main.OnTapCompleted += NextSlide;
        }

        public override void Hide()
        {
            base.Hide();
            ActivatePanel.OnShow -= Show;
            Core.InputManager.Main.OnTapCompleted -= NextSlide;
        }
    }
}
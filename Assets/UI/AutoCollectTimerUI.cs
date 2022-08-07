using System;
using UnityEngine;
using StarGarden.Stardust;

namespace StarGarden.UI
{
    public class AutoCollectTimerUI : MonoBehaviour
    {
        private bool active;

        private void Update()
        {
            if (!active)
            {
                if (AutoCollection.Active) Show();
                else return;
            }

            TimeSpan remaining = AutoCollection.Expiry - DateTime.Now;
            if (remaining < TimeSpan.Zero)
                Hide();

            print(remaining + " " + active);
        }

        private void Show()
        {
            active = true;
        }

        private void Hide()
        {
            active = false;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using StarGarden.Stardust;

namespace StarGarden.UI
{
    public class AutoCollectTimerUI : MonoBehaviour
    {
        public GameObject baseObject;

        private IEnumerator TimeUpdater()
        {
            TimeSpan remaining = AutoCollection.Expiry - DateTime.Now;
            while (remaining > TimeSpan.Zero)
            {
                remaining = AutoCollection.Expiry - DateTime.Now;
                print("remaining: " + remaining);

                yield return new WaitForSecondsRealtime(1f);
            }

            Deactivate();
        }

        private void Activate()
        {
            StartCoroutine(TimeUpdater());
            baseObject.SetActive(true);
        }

        private void Deactivate()
        {
            baseObject.SetActive(false);
        }

        private void OnEnable()
        {
            AutoCollection.OnAutoCollectionActivated += Activate;
            if (AutoCollection.Active)
                Activate();
        }

        private void OnDisable()
        {
            AutoCollection.OnAutoCollectionActivated -= Activate;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using StarGarden.Stardust;
using StarGarden.Core.SaveData;

namespace StarGarden.UI
{
    public class AutoCollectTimerUI : MonoBehaviour
    {
        public GameObject baseObject;
        public Image timerImage;

        private IEnumerator TimeUpdater()
        {
            TimeSpan remaining = AutoCollection.Expiry - DateTime.Now;
            while (remaining > TimeSpan.Zero)
            {
                remaining = AutoCollection.Expiry - DateTime.Now;
                print("remaining: " + remaining);
                float t = (float)remaining.TotalMinutes / (float)SaveDataManager.SaveData.LastAutoCollectSpan.TotalMinutes;
                timerImage.fillAmount = t;

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
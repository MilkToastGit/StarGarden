using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class HappinessBar : MonoBehaviour
    {
        public float current;
        public Image mask;

        void UpdateFillAmount()
        {
            float fillAmount = current;
            mask.fillAmount = fillAmount;
        }

        public void SetHappiness(float amount)
        {
            current = amount;
            UpdateFillAmount();
        }

        //placeholder
        public void ResetHappiness()
        {
            current = 0;
            UpdateFillAmount();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarGarden.UI
{
    public class HappinessBar : MonoBehaviour
    {
        public int maximum;
        public int current;
        public Image mask;

        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            GetCurrentFill();
        }

        void GetCurrentFill()
        {
            float fillAmount = (float)current / (float)maximum;
            mask.fillAmount = fillAmount;
        }

        public void CommonCookie()
        {
            current += 10;
        }
        
        public void CelestialCookie()
        {
            current += 30;
        }

        //placeholder
        public void ResetHappiness()
        {
            current = 0;
        }
    }
}

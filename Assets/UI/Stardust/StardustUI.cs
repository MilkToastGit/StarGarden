using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarGarden.Core;

namespace StarGarden.UI
{
    public class StardustUI : MonoBehaviour
    {
        public TextMeshProUGUI common, rare, mythical;

        private void UpdateUI()
        {
            common.text = ResourcesManager.Main.CommonStardust.ToString();
            rare.text = ResourcesManager.Main.RareStardust.ToString();
            mythical.text = ResourcesManager.Main.MythicalStardust.ToString();
        }

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            ResourcesManager.Main.OnStardustChanged += UpdateUI;
        }

        private void OnDisable()
        {
            ResourcesManager.Main.OnStardustChanged -= UpdateUI;
        }
    }
}
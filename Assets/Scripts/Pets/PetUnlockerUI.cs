using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarGarden.Pets;

namespace StarGarden.UI
{
    public class PetUnlockerUI : UIPanel
    {
        [SerializeField] private Image petImage;
        [SerializeField] private TextMeshProUGUI petName;

        public override void Show(object args)
        {
            if (args.GetType() != typeof(Pet))
                throw new System.Exception("Error: Type mismatch for args in method Show of script PetUnlocker. Expected: Pet");
            
            Pet pet = (Pet)args;
            petImage.sprite = pet.Sprite;
            petName.text = pet.Name;

            base.Show();
        }
    }
}
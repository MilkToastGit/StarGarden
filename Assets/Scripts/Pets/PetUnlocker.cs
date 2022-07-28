using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarGarden.Pets;

namespace StarGarden.UI
{
    public class PetUnlocker : MonoBehaviour
    {
        [SerializeField] private Image petImage;
        [SerializeField] private TextMeshProUGUI petName;
        private GameObject baseObject;

        private void Awake()
        {
            baseObject = transform.GetChild(0).gameObject;
        }

        public void Show(Pet pet)
        {
            baseObject.SetActive(true);
            petImage.sprite = pet.Sprite;
            petName.text = pet.Name;
        }

        public void Hide() => baseObject.SetActive(false);
    }
}
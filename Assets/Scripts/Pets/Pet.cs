using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    [CreateAssetMenu(fileName = "Pet", menuName = "StarGarden/Pet")]
    public class Pet : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public Sprite SignSprite;
        public Element Element;
        public string PersonalityTraits;
        [HideInInspector] public int PetIndex;
    }
}

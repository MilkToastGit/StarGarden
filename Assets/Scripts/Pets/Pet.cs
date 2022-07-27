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
        public GameObject Prefab;
        public Element Element;
        public string NegativeTrait;
        public GameObject NegativeEmote;
        public string NeutralTrait;
        public GameObject NeutralEmote;
        public string PositiveTrait;
        public GameObject PositiveEmote;
        [HideInInspector] public int PetIndex;
    }
}

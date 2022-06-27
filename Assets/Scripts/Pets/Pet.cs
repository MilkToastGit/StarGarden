using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    [CreateAssetMenu(fileName = "Pet", menuName = "StarGarden/Pet")]
    public class Pet : ScriptableObject
    {
        public Starsign Sign;
        public Element Element;
        public PersonalityTrait Behaviour1, Behaviour2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Pets
{
    [CreateAssetMenu(fileName = "Pet", menuName = "StarGarden/Pet")]
    public class Pet : ScriptableObject
    {
        public Starsign Sign;
        public Items.Hat EquippedHat;
        public Element Element;
        public PetBehaviour Behaviour1, Behaviour2;
    }
}

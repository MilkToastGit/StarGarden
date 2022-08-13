using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.UI
{
    public class InventorySack : MonoBehaviour
    {
        private static Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public static void SetState(bool active, bool open)
        {
            anim.SetBool("Active", active);
            anim.SetBool("Open", open);
        }
    }
}
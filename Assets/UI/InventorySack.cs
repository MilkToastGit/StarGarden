using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.UI
{
    public class InventorySack : MonoBehaviour
    {
        [SerializeField] private FMODUnity.StudioEventEmitter serialisedSackSound;
        private static Animator anim;
        private static FMODUnity.StudioEventEmitter sackSound;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            sackSound = serialisedSackSound;
        }

        public static void SetState(bool active, bool open)
        {
            anim.SetBool("Active", active);
            anim.SetBool("Open", open);
        }

        public static void ReturnToInventory()
        {
            SetState(false, false);
            sackSound.Play();
        }
    }
}
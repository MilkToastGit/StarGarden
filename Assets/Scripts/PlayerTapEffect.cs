using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden
{
    public class PlayerTapEffect : MonoBehaviour
    {
        [SerializeField] private GameObject[] pool;

        private Queue<GameObject> inactive = new Queue<GameObject>();
        private Queue<GameObject> active = new Queue<GameObject>();

        private void Awake()
        {
            foreach (GameObject go in pool)
                inactive.Enqueue(go);
        }

        private void ShowEffect()
        {
            GameObject effect = inactive.Dequeue();
            effect.transform.position = InputManager.Main.WorldTouchPosition;
            effect.SetActive(true);
            active.Enqueue(effect);
            Invoke("HideLast", 2f);
        }

        private void HideLast()
        {
            GameObject effect = active.Dequeue();
            effect.SetActive(false);
            inactive.Enqueue(effect);
        }

        private void OnEnable()
        {
            InputManager.Main.OnTapCompleted += pos => ShowEffect();
        }

        private void OnDisable()
        {
            InputManager.Main.OnTapCompleted -= pos => ShowEffect();
        }
    }
}
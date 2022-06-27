using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class IslandManager : MonoBehaviour
    {
        public static IslandManager Main { get; private set; }

        private void Awake()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public Island[] Islands => islands;
        [SerializeField] private Island[] islands;

        private void OnDrawGizmos()
        {
            foreach (Island i in Islands)
            {
                Color c = Color.red;
                switch (i.Element)
                {
                    case Element.Fire: c = Color.red; break;
                    case Element.Water: c = Color.blue; break;
                    case Element.Air: c = Color.yellow; break;
                    case Element.Earth: c = Color.green; break;
                }
                F.DrawRect(i.Bounds, c);
            }
        }

        public int WithinIsland(Vector2 point)
        {
            for (int i = 0; i < islands.Length; i++)
            {
                if (F.WithinBounds(point, islands[i].Bounds))
                    return i;
            }
            return -1;
        }
    }

    [System.Serializable]
    public class Island
    {
        public Rect Bounds;
        public Element Element;
    }
}
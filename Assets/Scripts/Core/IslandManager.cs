using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class IslandManager : MonoBehaviour, Manager
    {
        public static IslandManager Main { get; private set; }
        public LayerMask IslandMask;

        public int activeIsland;

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void LateInitialise() { }

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

        public int WithinIslandBounds(Vector2 point)
        {
            for (int i = 0; i < islands.Length; i++)
            {
                if (F.WithinBounds(point, islands[i].Bounds))
                    return i;
            }
            return -1;
        }

        public int WithinIsland(Vector2 point)
        {
            Collider2D col = Physics2D.OverlapPoint(point, IslandMask);
            if (!col) return -1;

            for (int i = 0; i < Islands.Length; i++)
                if (col.gameObject == Islands[i].IslandObject)
                    return i;

            return -1;
        }

        public void SetActiveIsland(int island)
        {
            if (activeIsland >= 0)
                islands[activeIsland].IslandObject.SetActive(false);
            islands[island].IslandObject.SetActive(true);

            activeIsland = island;
        }

        public void DisableActiveIsland()
        {
            if (activeIsland < 0) return;

            islands[activeIsland].IslandObject.SetActive(false);
            activeIsland = -1;
        }
    }

    [System.Serializable]
    public class Island
    {
        public GameObject IslandObject;
        public Rect Bounds;
        public Element Element;
    }
}
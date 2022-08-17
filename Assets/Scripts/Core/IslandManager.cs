using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class IslandManager : MonoBehaviour, Manager
    {
        public static IslandManager Main { get; private set; }
        public Island ActiveIsland => activeIsland < 0 ? null : islands[activeIsland];
        public Island[] Islands => islands;

        [SerializeField] private Stardust.StarfallSpawner starfallSpawner;
        [SerializeField] private LayerMask IslandMask;
        [SerializeField] private Island[] islands;

        private PreviewIsland[] previewIslands;
        private int activeIsland = -1;

        public void Initialise()
        {
            if (!Main)
            {
                Main = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            previewIslands = new PreviewIsland[islands.Length];
            for (int i = 0; i < islands.Length; i++)
            {
                islands[i].Index = i;
                previewIslands[i] = islands[i].IslandNavigationObject.GetComponent<PreviewIsland>();
            }

            starfallSpawner.OnActiveStarfallsChanged += UpdatePreviewIslandStarglow;
        }

        public void LateInitialise() { }


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

        public bool WithinIsland(Vector2 point, int island)
        {
            foreach (Island i in islands)
                i.IslandObject.SetActive(i.Index == island);

            int hit = WithinIsland(point);

            foreach (Island i in islands)
                i.IslandObject.SetActive(i.Index == activeIsland);

            return hit == island;

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

        public void UpdatePreviewIslandStarglow(List<Stardust.Starfall>[] active)
        {
            if (Stardust.AutoCollection.Active) return;

            for (int i = 0; i < previewIslands.Length; i++)
            {
                print($"[{i}] {active[i].Count > 0}");
                previewIslands[i].SetStarglowActive(active[i].Count > 0);
            }
        }

        public Island GetIslandFromElement(Element element)
        {
            foreach (Island i in islands)
                if (i.Element == element)
                    return i;
            throw new System.Exception($"Error: Island of element {element} does not exist.");
        }
    }

    [System.Serializable]
    public class Island
    {
        public GameObject IslandObject;
        public GameObject IslandNavigationObject;
        public Rect Bounds;
        public Element Element;
        public int Index;
    }
}
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

                Debug.DrawLine(new Vector2(i.Bounds.xMin, i.Bounds.yMin), new Vector2(i.Bounds.xMin, i.Bounds.yMax), c);
                Debug.DrawLine(new Vector2(i.Bounds.xMin, i.Bounds.yMax), new Vector2(i.Bounds.xMax, i.Bounds.yMax), c);
                Debug.DrawLine(new Vector2(i.Bounds.xMax, i.Bounds.yMax), new Vector2(i.Bounds.xMax, i.Bounds.yMin), c);
                Debug.DrawLine(new Vector2(i.Bounds.xMax, i.Bounds.yMin), new Vector2(i.Bounds.xMin, i.Bounds.yMin), c);
            }
        }
    }

    [System.Serializable]
    public class Island
    {
        public Rect Bounds;
        public Element Element;
    }
}
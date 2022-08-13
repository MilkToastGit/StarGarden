using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.LootBoxes
{
    [CreateAssetMenu(fileName = "ConstellationName", menuName = "StarGarden/Constellation")]
    public class Constellation : ScriptableObject, IEnumerable
    {
        [SerializeField] private ConstellationPoint[] Points;
        public ConstellationPoint this[int i] => Points[i];

        IEnumerator IEnumerable.GetEnumerator() => Points.GetEnumerator();
    }

    [System.Serializable]
    public class ConstellationPoint
    {
        public Vector2 Position;
        public float radius;
        public int[] NextPoints;
        public Vector3 Position3D => new Vector3(Position.x, Position.y, zPosition);

        private static int seed = 0;
        private float zPosition => Mathf.PerlinNoise(Position.x * 25f + seed, Position.y * 25f + seed) * 3 - 1.5f;
        private bool zPosSet = false;

        public static void RandomiseSeed() => seed = Random.Range(-10000, 10000);
    }
}
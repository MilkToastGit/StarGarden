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
        //{
        //    get
        //    {
        //        if (!zPosSet)
        //        {
        //            //zPosition = Random.Range(-2f, 2f);
        //            zPosition = Mathf.PerlinNoise(Position.x / 100f, Position.y / 100f) / 100f;
        //            zPosSet = true;
        //        }
        //        return new Vector3(Position.x, Position.y, zPosition);
        //    }
        //}

        private float zPosition => Mathf.PerlinNoise(Position.x * 25f, Position.y * 25f) * 3 - 1.5f;
        private bool zPosSet = false;
    }
}
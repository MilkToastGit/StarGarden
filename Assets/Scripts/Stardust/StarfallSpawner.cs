using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;

namespace StarGarden.Stardust
{
    public class StarfallSpawner : MonoBehaviour
    {
        public GameObject starFall;

        private void Start()
        {
            Invoke();
        }

        private void Invoke() => Invoke("SpawnStarfall", Random.Range(2, 3));//Random.Range(15, 60));

        private void SpawnStarfall()
        {
            Island island = IslandManager.Main.Islands[0];//.Random();
            float randX = Random.Range(island.Bounds.xMin, island.Bounds.xMax);
            float randY = Random.Range(island.Bounds.yMin, island.Bounds.yMax);
            Vector2 point = new Vector2(randX, randY);

            Vector2 centreBound = (island.Bounds.center - point).normalized;
            for (int i = 0; IslandManager.Main.WithinIsland(point) < 0 && i < 20; i++)
                point += centreBound * 0.5f;

            Instantiate(starFall, point, Quaternion.identity, transform);

            Invoke();
        }
    }
}
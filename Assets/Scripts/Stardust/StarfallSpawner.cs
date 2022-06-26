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

        private void Invoke() => Invoke("SpawnStarfall", Random.Range(5, 10));//Random.Range(15, 60));

        private void SpawnStarfall()
        {
            Island island = IslandManager.Main.Islands.Random();
            float randX = Random.Range(island.Bounds.xMin, island.Bounds.xMax);
            float randY = Random.Range(island.Bounds.yMin, island.Bounds.yMax);

            Instantiate(starFall, new Vector2(randX, randY), Quaternion.identity, transform);

            Invoke();
        }
    }
}
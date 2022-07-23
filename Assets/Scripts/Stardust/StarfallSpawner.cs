using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Core.SaveData;

namespace StarGarden.Stardust
{
    public class StarfallSpawner : MonoBehaviour, Manager
    {
        public GameObject starFall;

        public void Initialise()
        {
            Invoke();
        }

        public void LateInitialise()
        {
            System.TimeSpan sinceLastSave = System.DateTime.Now - SaveDataManager.SaveData.LastSave;
            float totalHours = (float)sinceLastSave.TotalHours;
            float timeMult = F.Map(totalHours, 0f, 12f);
            float maxSpawnAmount = Random.Range(20f, 35f);

            print($"Time since last: {sinceLastSave.Hours}:{sinceLastSave.Minutes}:{sinceLastSave.Seconds}");
            print($"Spawn Amount: {maxSpawnAmount} x {timeMult} = {maxSpawnAmount * timeMult}");

            for (int i = 0; i < maxSpawnAmount * timeMult; i++)
                SpawnStarfall();
        }

        private void Invoke() => Invoke("SpawnStarfall", Random.Range(15, 60));//Random.Range(15, 60));

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
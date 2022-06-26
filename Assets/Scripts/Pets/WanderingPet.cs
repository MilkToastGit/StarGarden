using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.Pets
{
    public class WanderingPet : MonoBehaviour
    {
        public Pet Pet;
        private int currentIsland;
        private float speed;

        private Vector2 RandomDirection()
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private Vector2 GridCast(Vector2 direction, float maxDistance)
        {
            DecorationInstances[] allItems = InventoryManager.Main.GetAllItemsFromCategory(0) as DecorationInstances[];

            direction.Normalize();
            float increment = Mathf.Min(WorldGrid.Spacing.x, WorldGrid.Spacing.y);
            float distance = 0;
            Vector2 lastPoint = transform.position;
            while (distance < maxDistance)
            {
                Vector2 nextPoint = lastPoint += direction * increment;
                distance += increment;

                if (IslandManager.Main.WithinIsland(nextPoint) != currentIsland)
                    return lastPoint;

                foreach (DecorationInstances instance in allItems)
                    if (instance.IsPositionTaken(WorldGrid.WorldToGrid(nextPoint)))
                        return lastPoint;

                lastPoint = nextPoint;
            }

            return lastPoint;
        }

        private IEnumerator BehaviourCycle()
        {
            Vector2 target = GridCast(RandomDirection(), 5);
            Vector2 direction = target.normalized;
            float distanceTravelled = 0;
            while (distanceTravelled < 5)
            {
                distanceTravelled += Time.deltaTime * speed;
                if (distanceTravelled > 5)
                transform.position += (Vector3)direction * Time.deltaTime * speed;
            }
        }

        public enum State
        {
            Idle,
            Wandering,
            Emoting
        }
    }
}
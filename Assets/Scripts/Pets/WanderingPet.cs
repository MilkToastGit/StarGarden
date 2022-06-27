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
        [SerializeField]private float speed;
        private int currentIsland;

        private void Start()
        {
            StartCoroutine(BehaviourCycle());
        }

        private Vector2 RandomDirection()
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        private Vector2 GridCast(Vector2 direction, float maxDistance)
        {
            ItemInstances[] allItems = InventoryManager.Main.GetAllItemsFromCategory(0);

            direction.Normalize();
            float increment = Mathf.Min(WorldGrid.Spacing.x, WorldGrid.Spacing.y);
            float distance = 0;
            Vector2 lastPoint = transform.position;
            while (distance < maxDistance)
            {
                Vector2 nextPoint = lastPoint + direction * increment;
                distance += increment;

                if (IslandManager.Main.WithinIsland(nextPoint) != currentIsland)
                    return lastPoint;

                Debug.DrawRay(WorldGrid.GridToWorld(WorldGrid.WorldToGrid(nextPoint)), Vector2.up / 5, Color.white, 7);

                foreach (DecorationInstances instance in allItems)
                    if (instance.IsPositionTaken(WorldGrid.WorldToGrid(nextPoint)))
                        return lastPoint;

                lastPoint = nextPoint;
            }

            return lastPoint;
        }

        private IEnumerator BehaviourCycle()
        {
            while (true)
            {
                Vector2 target = GridCast(RandomDirection(), Random.Range(0.5f, 3f));
                Debug.DrawRay(target, Vector2.up, Color.green, 7);
                Vector2 direction = (target - (Vector2)transform.position).normalized;
                float maxDistance = (target - (Vector2)transform.position).magnitude;
                float distanceTravelled = 0;
                while (distanceTravelled < maxDistance)
                {
                    float moveAmount = Time.deltaTime * speed;
                    distanceTravelled += moveAmount;
                    if (distanceTravelled > maxDistance)
                    {
                        transform.position = target;
                        break;
                    }

                    transform.position += (Vector3)direction * moveAmount;
                    yield return null;
                }

                yield return new WaitForSeconds(Random.Range(1f, 5f));
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Items;

namespace StarGarden.Pets
{
    public class WanderingPet : MonoBehaviour, Interactable
    {
        public Pet Pet;
        public Transform[] hatParent;

        private HatInstances equippedHat;
        [SerializeField]private float speed;
        private int currentIsland;
        private Animator anim;
        private SpriteRenderer sprite;

        public bool Passthrough => false;

        private void Start()
        {
            anim = GetComponent<Animator>();
            sprite = GetComponentInChildren<SpriteRenderer>();

            StartCoroutine(BehaviourCycle());
            SpawnHat();
        }

        private void SpawnHat()
        {
            foreach(Transform t in hatParent)
                if (equippedHat != null)
                    Instantiate(equippedHat.Item.Prefab, t);
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
                Vector2 direction = (target - (Vector2)transform.position).normalized;
                Debug.DrawRay(target, Vector2.up, Color.green, 7);

                float maxDistance = (target - (Vector2)transform.position).magnitude;
                if (maxDistance <= 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                anim.SetBool("Walking", true);
                sprite.flipX = direction.x > 0;
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
                anim.SetBool("Walking", false);

                yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));

                if (Random.value > 0.7f)
                {
                    anim.SetInteger("Emote", Random.Range(0, 3));
                    anim.SetTrigger("EmoteTrigger");

                    yield return new WaitForSeconds(Random.Range(2.5f, 3f));
                    anim.SetTrigger("UnemoteTrigger");
                    yield return new WaitForSeconds(1f);
                }
                else yield return new WaitForSeconds(Random.Range(2.5f, 3f));
            }

        }

        public void OnStartTouch()
        {
            
        }

        public void OnEndTouch()
        {
            
        }

        public enum State
        {
            Idle,
            Wandering,
            Emoting
        }
    }
}
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
        public bool flip;
        [SerializeField] private Transform hatParentBase;
        [SerializeField] private float speed;

        [HideInInspector] public Vector2[] DefaultHatPositions;
        [HideInInspector] public Quaternion[] DefaultHatRotations;
        [HideInInspector] public Vector2[] DefaultHatScales;

        [HideInInspector] public float Happiness;
        public HatInstances EquippedHat;
        private Transform[] hatParents;
        private int currentIsland;
        private Animator anim;
        private SpriteRenderer sprite;

        public bool Passthrough => false;

        public void Initialise()
        {
            anim = GetComponent<Animator>();
            sprite = GetComponentInChildren<SpriteRenderer>();

            hatParents = new Transform[hatParentBase.childCount];
            for (int i = 0; i < hatParentBase.childCount; i++)
                hatParents[i] = hatParentBase.GetChild(i).transform;

            DefaultHatPositions = new Vector2[hatParents.Length];
            DefaultHatRotations = new Quaternion[hatParents.Length];
            DefaultHatScales = new Vector2[hatParents.Length];
            for (int i = 0; i < hatParents.Length; i++)
            {
                DefaultHatPositions[i] = hatParents[i].localPosition;//GetComponentInChildren<PetSpriteShifter>().sprites[0].LocalHatPositions;
                DefaultHatRotations[i] = hatParents[i].localRotation;
                DefaultHatScales[i] = hatParents[i].localScale;
            }
        }

        private void Start()
        {
            StartCoroutine(BehaviourCycle());
        }

        public void SetHat(HatInstances hat)
        {
            EquippedHat = hat;
            foreach (Transform t in hatParents)
            {
                t.Order66();
                if (EquippedHat != null)
                {
                    Transform spawnedHat = Instantiate(EquippedHat.Item.Prefab, t).transform;
                    spawnedHat.localPosition = Vector3.zero;
                    spawnedHat.localRotation = Quaternion.identity;
                    spawnedHat.localScale = Vector3.one;
                }
            }
        }

        public void IncreaseHappiness(float amount)
        {
            Happiness = Mathf.Min(Happiness + amount, 1);
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

        private void FlipX(bool flip)
        {
            sprite.flipX = flip;

            Vector2 scale = hatParentBase.localScale;
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
            hatParentBase.localScale = scale;
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
                if (flip)
                    FlipX(direction.x > 0);

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
                    // Lower chance for neutral emote
                    if (Random.value > 0.6f)
                        anim.SetInteger("Emote", 1);
                    // Emote negative or positive based on happiness
                    else
                        anim.SetInteger("Emote", Random.value < Happiness ? 2 : 0);
                    anim.SetTrigger("EmoteTrigger");

                    yield return new WaitForSeconds(Random.Range(2.5f, 3f));
                    anim.SetTrigger("UnemoteTrigger");
                    yield return new WaitForSeconds(1f);
                }
                else yield return new WaitForSeconds(Random.Range(2.5f, 3f));
            }

        }

        public void OnTap()
        {
            UI.UIManager.Main.ShowPetMenu(Pet.PetIndex);
        }

        public void OnStartTouch() { }

        public void OnEndTouch() { }

        public enum State
        {
            Idle,
            Wandering,
            Emoting
        }
    }
}
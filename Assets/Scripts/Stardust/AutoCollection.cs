using System;
using UnityEngine;
using StarGarden.Core;
using StarGarden.Core.SaveData;

namespace StarGarden.Stardust
{
    public class AutoCollection : MonoBehaviour, Manager
    {
        [SerializeField] private int commonCost, rareCost, mythicalCost;
        [SerializeField] private int commonMinutes, rareMinutes, mythicalMinutes;

        public delegate void AutoCollectionDeactivatedEvent();
        public static event AutoCollectionDeactivatedEvent OnAutoCollectionActivated;

        public static DateTime Expiry => expiry;
        public static bool Active => expiry - DateTime.Now > TimeSpan.Zero;

        private static DateTime expiry;
        [SerializeField] private AnimationCurve spawnRandomisationCurve;

        public void Initialise() { }

        public void LateInitialise()
        {
            AllSaveData data = SaveDataManager.SaveData;
            expiry = data.AutoCollectExpiry; 
            
            AddIdleStardust();
        }

        private void AddIdleStardust()
        {
            AllSaveData data = SaveDataManager.SaveData;
            
            float idleCollectDuration = (float)IdleAutoCollectDuration(SaveDataManager.LastSessionSaveDate).TotalSeconds;
            //print($"calculated: {idleCollectDuration}");
            if (idleCollectDuration <= 0) return;

            float spawnRate = spawnRandomisationCurve.Evaluate(UnityEngine.Random.value)
                .Map(0f, 1f, StarfallSpawner.SecondPerStarfallRange.x, StarfallSpawner.SecondPerStarfallRange.y);
            int collectedAmount = Mathf.FloorToInt(idleCollectDuration / spawnRate);
            //print($"minPerSecond: {StarfallSpawner.SecondPerStarfallRange}, rate: {spawnRate}, collected: {collectedAmount}");

            float lastHappiness = GetSavedHappiness(data.PetSaveData);
            float currentHappiness = Pets.PetManager.Main.CollectiveHappiness;
            float averageHappiness = (lastHappiness + currentHappiness) / 2f;

            float rareRatio = averageHappiness.Map(0f, 1f, Starfall.RareRateRange.x, Starfall.RareRateRange.y);

            ResourcesManager.Main.AddStardust(Rarity.Common, Mathf.RoundToInt(collectedAmount * (1f - rareRatio)));
            ResourcesManager.Main.AddStardust(Rarity.Rare, Mathf.RoundToInt(collectedAmount * rareRatio));
        }

        public TimeSpan IdleAutoCollectDuration(DateTime lastSave)
        {
            //print($"now: {DateTime.Now}, expiry: {expiry}, lastSave: {lastSave}");
            //print(DateTime.Now < expiry);
            if (DateTime.Now < expiry)
                return DateTime.Now - lastSave;
            else
                return expiry - lastSave;
        }

        private void AddTime(TimeSpan amount)
        {
            if (DateTime.Now > expiry)
            {
                OnAutoCollectionActivated?.Invoke();
                expiry = DateTime.Now;
            }

            //print(DateTime.Now > expiry);
            //print(expiry);

            expiry += amount;
            SaveDataManager.SaveData.AutoCollectExpiry = expiry;
            SaveDataManager.SaveAll();
        }

        public void Purchase(int rarity)
        {
            int cost = 0;
            int minutes = 0;

            switch (rarity)
            {
                case 0:
                    cost = commonCost;
                    minutes = commonMinutes;
                    break;
                case 1:
                    cost = rareCost;
                    minutes = rareMinutes;
                    break;
                case 2:
                    cost = mythicalCost;
                    minutes = mythicalMinutes;
                    break;
            }

            if (!ResourcesManager.Main.TryPurchase(Rarity.Mythical, cost))
            {
                print("NOT ENOUGH STARDUST");
                return;
            }

            AddTime(new TimeSpan(0, minutes, 0));
        }

        private float GetSavedHappiness(PetSaveData[] data)
        {
            float h = 0f;
            int active = 0;
            foreach (PetSaveData pet in data)
                if (pet.Obtained)
                {
                    h += pet.Happiness;
                    active++;
                }
            return h / active;
        }
    }
}
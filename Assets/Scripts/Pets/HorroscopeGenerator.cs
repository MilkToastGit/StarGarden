using System;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Pets
{
    public class HorroscopeGenerator : MonoBehaviour, Manager
    {
        [SerializeField] private string[] serialisedHorroscopes;
        private static string[] Horroscopes;

        private static int[] pickedHorroscopes;

        public static string GetHorroscope(Starsign sign) => Horroscopes[pickedHorroscopes[(int)sign]];

        public void Initialise() 
        {
            Horroscopes = serialisedHorroscopes;
        }

        public void LateInitialise()
        {
            DateTime lastDate = new DateTime(
                SaveDataManager.SaveData.LastSave.Year,
                SaveDataManager.SaveData.LastSave.Month,
                SaveDataManager.SaveData.LastSave.Day);

            if (lastDate != DateTime.Today)
                PickHorroscopes();
            else
                pickedHorroscopes = SaveDataManager.SaveData.PickedHorroscopes;
        }

        private static void PickHorroscopes()
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < Zodiac.Zodiacs.Length; i++)
                numbers.Add(i);

            List<int> picked = new List<int>();
            for (int i = 0; i < Zodiac.Zodiacs.Length; i++)
            {
                int pickIndex = UnityEngine.Random.Range(0, numbers.Count);
                picked.Add(numbers[pickIndex]);
                numbers.RemoveAt(pickIndex);
            }

            pickedHorroscopes = picked.ToArray();
            SaveDataManager.SaveData.PickedHorroscopes = pickedHorroscopes;
            SaveDataManager.SaveAll();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Tutorial
{
    public class TutorialManager : MonoBehaviour, Manager
    {
        public Tutorial[] tutorials;

        public void Initialise() { }

        public void LateInitialise()
        {
            bool[] completed = SaveDataManager.SaveData.TutorialsCompleted;
            for (int i = 0; i < tutorials.Length; i++)
            {
                tutorials[i].Completed = completed[i];
                if (!completed[i])
                    tutorials[i].OnCompleted += UpdateTutorialSaveData;
            }
        }

        private void UpdateTutorialSaveData(Tutorial tutorial)
        {
            bool[] completed = new bool[tutorials.Length];
            for (int i = 0; i < tutorials.Length; i++)
                completed[i] = tutorials[i].Completed;

            SaveDataManager.SaveData.TutorialsCompleted = completed;
            SaveDataManager.SaveAll();

            tutorial.OnCompleted -= UpdateTutorialSaveData;
        }
    }
}
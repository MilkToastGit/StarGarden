using System;
using UnityEngine;
using StarGarden.Core.SaveData;

namespace StarGarden.Tutorial
{
    public class SetBirthday : MonoBehaviour
    {
        public UI.DateSelector date;
        public void Set()
        {
            SaveDataManager.SaveData.UserBirthdate = date.SelectedDate;
            SaveDataManager.SaveAll();
        }
    }
}

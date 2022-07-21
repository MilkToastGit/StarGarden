using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;
using StarGarden.Items;

public class SaveLoadTester : MonoBehaviour
{
    public void SaveAll()
    {
        SaveDataManager.SaveAll();
    }

    //public void LoadItems()
    //{
    //    SaveDataManager.ReadAll();
    //}
}

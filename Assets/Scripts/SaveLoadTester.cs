using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;
using StarGarden.Items;

public class SaveLoadTester : MonoBehaviour
{
    public void SaveItems()
    {
        SaveDataManager.SaveItemData();
    }

    public void LoadItems()
    {
        SaveDataManager.ReadItemData();
    }
}

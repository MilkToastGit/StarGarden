using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarGarden.Core.SaveData;
using StarGarden.Items;

public class SaveLoadTester : MonoBehaviour
{
    public void SaveAll()
    {
        SaveDataManager.SavePetData();
        SaveDataManager.SaveItemData();
    }

    public void LoadItems()
    {
        SaveDataManager.ReadItemData();
    }
}

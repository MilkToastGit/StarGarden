using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptExecutioner : MonoBehaviour
{
    public GameObject[] Managers;

    private void Awake()
    {
        foreach (GameObject manager in Managers)
            manager.GetComponent<Manager>().Initialise();
    }
}

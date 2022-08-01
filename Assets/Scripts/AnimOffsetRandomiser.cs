using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOffsetRandomiser : MonoBehaviour
{
    [SerializeField] private string paramName;
    private void OnEnable() => GetComponent<Animator>().SetFloat(paramName, Random.value);
}

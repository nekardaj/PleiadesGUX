using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ParametersInstance", order = 1)]
public class ParameterSettings : ScriptableObject
{
    public int SpawnInterval;
    public float Speed;
}

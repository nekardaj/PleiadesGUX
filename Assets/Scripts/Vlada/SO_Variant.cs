using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewVariant", menuName = "Game_Variants/GameVariant", order = 1)]
public class SO_Variant : ScriptableObject
{
    public VariantType VariantType;
    public float FlockSpeed;
    public float FlockRotationSpeed;
    public float FlockReturnSpeed;
    public float MaxAngle;
    public float StarReward;
}

public enum FlockType
{
    Default
}

public enum VariantType
{
    Slow,
    Fast
}

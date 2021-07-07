using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CircuitType
{
    round,
    perdilio,
    question,
    pelmenni
}

[System.Serializable]
public class circuitDefinitions
{
    public string circuit_name;
    public CircuitType circType;
    public int amountOfLaps;
    public Vector3 HeroSpawnTransform;
    public GameObject circuitGO;
}


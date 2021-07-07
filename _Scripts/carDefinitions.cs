using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CarType
{
    govnarri,
    troika,
    twotwoeight,
    natasha,
    bylka,
    sema
}

[System.Serializable]
public class carDefinitions
{
    public string car_name;
    public CarType carType;
    public Sprite car_Prefab;
    public bool enabled;
    public float numberOfEnable;
    public float botDificulty;

    [Header("1.speed 2.rotSpeed 3.grassdebaff")]
    public List<float> properties;
}

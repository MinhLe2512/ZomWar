using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData", order = 1)]
public class GunData : ScriptableObject
{
    public float Range;
    public float Cooldown;
    public float ConeAngle;
}

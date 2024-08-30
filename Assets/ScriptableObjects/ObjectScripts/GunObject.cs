using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunObject", menuName = "ScriptableObjects/GunObject", order = 1)]
public class GunObject : ScriptableObject
{
    public string gunName;
    public float flatShellSpeed; // Speed for flat trajectory
    public float muzzleVelocity; // Muzzle velocity for ballistic trajectory
    public float gunReload;
    public GameObject shellPrefab;
}

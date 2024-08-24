using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShellObject", menuName = "ScriptableObjects/ShellObject", order = 2)]
public class ShellObject : ScriptableObject
{
    public bool AP;
    public float shellAPPenetration;
    public float shellHEPenetration;
    public float shellAPDamage;
    public float shellHEDamage;
}

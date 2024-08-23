using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct TurretSlot 
{
    public Vector2 LocalPosition;
    public float LocalEurlerZ;
    public TurretComponent Turret;

    public TurretSlot(Vector2 rel_pos, float rel_forward) {
        LocalPosition = rel_pos;
        LocalEurlerZ = rel_forward;
        Turret = null;
    }
}
// TODO: make a turretslot struct that holds turrent slot attributes:
/*
    - turret boat-relative position
    - turret boat-relative forward-angle i.e. how many degrees off from boat-forward is the default direction of this turret
    - #IDEAS
        * turret slot sizes (only certain turrets can fit here)
        * 
*/
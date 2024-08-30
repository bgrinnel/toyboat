using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A struct that holds information related to turret component default positions and angles
/// </summary>
[Serializable]
public struct TurretSlot 
{

    public Vector2 localPosition;

    /// <summary>
    /// The default Y of the turret in this slot that specifies the bounds TurretComponent.fireRotationDegrees is based on
    /// </summary>
    public float initLocalAngle;
    
    /// <summary>
    /// a Vector2 package of minimum roation and maximum rotation
    /// </summary>
    public Vector2 angleMinMax {
        get 
        { 
            return new Vector2(
                initLocalAngle - turretComp.RotationDegrees(),
                initLocalAngle + turretComp.RotationDegrees()
            );
        }
    }
    public TurretComponent turretComp;

    public TurretSlot(Vector2 rel_pos, float rel_forward) 
    {
        localPosition = rel_pos;
        initLocalAngle = rel_forward;
        turretComp = null;
    }

    public float currAngle 
    {
        get { return turretComp.transform.eulerAngles.y; }
    }
    public float currLocalAngle 
    {
        get { return turretComp.transform.localEulerAngles.y; }
    } 
    
    public Vector3 currPosition 
    {
        get { return turretComp.transform.position; }
    }

    public bool isReloading 
    {
        get { return turretComp.IsReloading(); }
    }

    public float rotationSpeed {
        get { return turretComp.RotationSpeed(); }
    }

    public void SetCurrLocalAngle(float angle)
    {
        Transform t = turretComp.transform;
        Vector3 eurlers = t.localRotation.eulerAngles;
        t.SetLocalPositionAndRotation(t.localPosition, Quaternion.Euler(eurlers.x, angle, eurlers.z));
    }
    public void Fire(Vector3 target) 
    {
        turretComp.Fire(target);
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
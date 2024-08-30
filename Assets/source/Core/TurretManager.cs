using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// A manager class that holds logic related to turret firing and targeting
/// </summary>
public class TurretManager
{
    /// <summary>
    /// The list of active targets to manage turret aiming between (i.e. future-proofing for smarter targeting)
    /// </summary>
    private List<Transform> targetList;

    /// <summary>
    /// A simple way of keeping the future proofing whilst making the code look readable while it's still simple
    /// resolves and sets as the first Transform in targetList
    /// </summary>
    private Transform simpleTarget {
        get { return targetList[0]; }
        set { targetList[0] = value; }
    }

    public Transform boatTransform;

    public TurretManager(Transform boat)
    {
        boatTransform = boat;
        targetList = new List<Transform>();
        targetList.Append(null); // setting it so simpleTarget is always a valid fetch
    }

    // Update is called once per frame
    public void Update(TurretSlot[] turrets)
    {
        if (!simpleTarget) return; // don't bother if we haven't set a target
        foreach (TurretSlot turret in turrets)
        {
            float angleDiff = RotateTurret(turret, GetAngleToTarget(turret, simpleTarget.position));
            if (simpleTarget != null && Mathf.Abs(angleDiff) < 0.5f)
            {
                turret.Fire(simpleTarget.position);
            }
        };
    }

    Vector3 CalculateTargetLeadPosition(Transform target, float shellSpeed)
    {
        var targetlastposition = target.position;
        Vector3 targetVelocity = (target.position - targetlastposition) / Time.deltaTime;

        var targetDistance = Vector3.Distance(boatTransform.position, target.position);
        var balisticTime = targetDistance/shellSpeed;

        var targetLeadPosition = target.position + targetVelocity * balisticTime;
        return targetLeadPosition;
    }

    float GetAngleToTarget(TurretSlot turret, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - turret.currPosition; 
        // WARNING: should we change the first atan2 arg to z and not y bc I thought that was right
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        targetAngle -= boatTransform.eulerAngles.z;
        float targetLocalAngle = Mathf.DeltaAngle(turret.initLocalAngle, targetAngle);
        return targetLocalAngle;
    }
    float RotateTurret(TurretSlot turret, float targetLocalAngle)
    {
        float angleMin = turret.angleMinMax.x;
        float angleMax = turret.angleMinMax.y;
        float netAngles = turret.currLocalAngle - turret.initLocalAngle;
        float angleDifference = Mathf.DeltaAngle(netAngles, targetLocalAngle);
        float turret_angle = turret.currLocalAngle;
        if (Mathf.Abs(targetLocalAngle) > angleMax)
        {
            Debug.Log("Status 1");
            float rotationStep = turret.rotationSpeed * Time.deltaTime;
            if (targetLocalAngle > 0)
            {
                turret_angle += rotationStep;
            }
            else if (targetLocalAngle < 0)
            {
                turret_angle -= rotationStep;
            }
            turret_angle = Mathf.Clamp(turret_angle, angleMin, angleMax);
        }
        else
        {
            float rotationStep = Mathf.Sign(angleDifference) * turret.rotationSpeed * Time.deltaTime;
            Debug.Log("Sign Value: " + Mathf.Sign(angleDifference));
            turret_angle = Mathf.Clamp(turret_angle + rotationStep, angleMin, angleMax);
        }
        turret.SetCurrLocalAngle(turret_angle);
        return angleDifference;
    }
}

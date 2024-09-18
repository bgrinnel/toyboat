using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSystem : MonoBehaviour
{
    public Transform target; // The target to follow
    public Transform ship; //the ship;'s transform, not the hull
    public float rotationSpeed = 5f; // Rotation speed
    public float maxRotationAngle = 130f; // Max rotation angle from the initial forward direction
    public List<Gun> turretGuns = new List<Gun>();

    [Range (0, 180)]
    private float currentAngle = 0f;
    private float startAngle; // The initial angle of the turret
    private float angleDifference;
    private Transform primaryTarget;

    void Start()
    {
        GameModeObject.Register(this);
        startAngle = transform.localEulerAngles.y; //set initial turret angle
        currentAngle = transform.localEulerAngles.y; //set curret turret angle
    }

    // Update is called once per frame
    void Update()
    {
        TargetFinding();
        TurretRotation(Aiming(target.position));
        AutoFiring();
    }

    void AutoFiring()
    {
        if (target != null && Mathf.Abs(angleDifference) < 0.5f)
        {
            foreach (Gun gun in turretGuns)
            {
                gun.Fire(target.position);
            }
        }
    }
    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void TargetFinding()
    {
        var distance = 0f;//Default Distnace
        if (primaryTarget == null)//if No enemy is designated as targert
        {
            
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))//go through all the enemies present
            {
                if ((Mathf.Abs(Aiming(enemy.transform.position)) < maxRotationAngle))//if there is enemies in firing angle
                {
                    if (distance <= 0f)//if there is not target designated
                    {
                        target = enemy.transform;
                        distance = Vector3.Distance(transform.position, enemy.transform.position);
                    }
                    else if (distance > Vector3.Distance(transform.position, enemy.transform.position) && target == null)//if there is a enemy closser than the current one and the original target is destroyed
                    {
                        distance = Vector3.Distance(transform.position, enemy.transform.position);
                        target = enemy.transform;
                    }
                }
            }
        }
        
        else//if there is a designeated target
        {
            if (Mathf.Abs(Aiming(primaryTarget.transform.position)) > maxRotationAngle)//if the deisgnated target is out of firing angle
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))//Go through all enemies present
                {
                    if ((Mathf.Abs(Aiming(enemy.transform.position)) < maxRotationAngle))//if there is enemies within the firing angle
                    {
                        if (distance <= 0f)//if there is not target designated
                        {
                            target = enemy.transform;
                            distance = Vector3.Distance(transform.position, enemy.transform.position);
                        }
                        else if (distance > Vector3.Distance(transform.position, enemy.transform.position))//if there is a enemy closser than the current one and the original target is destroyed
                        {
                            distance = Vector3.Distance(transform.position, enemy.transform.position);
                            target = enemy.transform;
                        }
                    }
                }
            }
            else//if the designated target is within the firing angle
            {
                target = primaryTarget;//make the current target as the primary target
            }
        }
    }

    Vector3 calculateTargetLeadPosition(Transform target, float shellSpeed)
    {
        var targetlastposition = target.position;
        Vector3 targetVelocity = (target.position - targetlastposition) / Time.deltaTime;

        var targetDistance = Vector3.Distance(transform.position, target.position);
        var balisticTime = targetDistance/shellSpeed;

        var targetLeadPosition = target.position + targetVelocity * balisticTime;
        return targetLeadPosition;
    }

    float Aiming(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position; 
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        targetAngle -= ship.eulerAngles.y;
        float targetLocalAngle = Mathf.DeltaAngle(startAngle, targetAngle);
        return targetLocalAngle;
    }
    void TurretRotation(float targetLocalAngle)
    {
        var turretMax = maxRotationAngle + startAngle;
        var turretMin = -maxRotationAngle + startAngle;
        float netAngles = transform.localEulerAngles.y - startAngle;
        angleDifference = Mathf.DeltaAngle(netAngles, targetLocalAngle);
        if (Mathf.Abs(targetLocalAngle) > maxRotationAngle)
        {
            //Debug.Log("Status 1");
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (targetLocalAngle > 0)
            {
                currentAngle += rotationStep;
            }
            else
            {
                currentAngle -= rotationStep;
            }
            currentAngle = Mathf.Clamp(currentAngle, turretMin, turretMax);
        }

        else
        {
            float rotationStep = Mathf.Sign(angleDifference) * rotationSpeed * Time.deltaTime;
            //Debug.Log("Sign Value: " + Mathf.Sign(angleDifference));
            currentAngle = Mathf.Clamp(currentAngle + rotationStep, turretMin, turretMax);
        }
        transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }

    void OnDestroyed()
    {
        GameModeObject.Unregister(this);
    }
}

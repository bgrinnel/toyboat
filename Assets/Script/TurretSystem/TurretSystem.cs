using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSystem : MonoBehaviour
{
    public Transform target; // The target to follow
    public Transform ship; //the ship;'s transform, not the hull
    public float rotationSpeed = 5f; // Rotation speed
    public float maxRotationAngle = 130f; // Max rotation angle from the initial forward direction

    [Range (0, 180)]
    private float currentAngle = 0f;
    private float startAngle; // The initial angle of the turret

    void Start()
    {
        startAngle = transform.localEulerAngles.z;
        currentAngle = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        TurretRotation(Aiming().x, Aiming().y);
    }
    Vector2 Aiming()
    {
        Vector3 direction = target.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle -= ship.eulerAngles.z;
        float targetLocalAngle = Mathf.DeltaAngle(startAngle, targetAngle);
        Debug.Log("Target Local Angles:" + targetLocalAngle);
        return new Vector2 (targetLocalAngle, targetAngle);
    }
    void TurretRotation(float targetLocalAngle, float targetAngle)
    {
        var turretMax = maxRotationAngle + startAngle;
        Debug.Log("TurretMax: " + turretMax);
        var turretMin = -maxRotationAngle + startAngle;
        Debug.Log("TurretMin: " + turretMin);
        float netAngles = transform.localEulerAngles.z - startAngle;
        float angleDifference = Mathf.DeltaAngle(netAngles, targetLocalAngle);
        Debug.Log("angle Difference: " + angleDifference);
        if (Mathf.Abs(targetLocalAngle) > maxRotationAngle)
        {
            Debug.Log("Status 1");
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
        /*else
        {
            Debug.Log("Status 2");
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (angleDifference != 0)
            {
                if (angleDifference > 0)
                {
                    currentAngle -= rotationStep;
                }
                else
                {
                    currentAngle += rotationStep;
                }
            }
        }*/
        else
        {
            float rotationStep = Mathf.Sign(angleDifference) * rotationSpeed * Time.deltaTime;
            Debug.Log("Status 3");
            currentAngle = Mathf.Clamp(currentAngle + rotationStep, turretMin, turretMax);
        }
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}

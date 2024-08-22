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

    void RotateTurret()
    {
        Vector2 directionToTarget = target.position - transform.position;
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        targetAngle -= ship.rotation.z;

        // Calculate the difference between the current angle and the target angle
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
        // Determine if the angle difference is within the range to rotate directly
        if (Mathf.Abs(angleDifference) > maxRotationAngle)
        {
            // If the target is on the other side, rotate the turret around in the longer direction
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (targetAngle <= 0)
            {
                currentAngle -= rotationStep;
            }
            else
            {
                currentAngle += rotationStep;
            }
        }
        else if (Mathf.Abs(targetAngle) > maxRotationAngle)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (targetAngle <= 0) { currentAngle -= rotationStep; }
            else { currentAngle += rotationStep; }
            currentAngle = Mathf.Clamp(currentAngle, -maxRotationAngle, maxRotationAngle);
        }
        else
        {
            // Rotate toward the target normally
            float rotationStep = Mathf.Sign(angleDifference) * rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.Clamp(currentAngle + rotationStep, -maxRotationAngle, maxRotationAngle);
        }

        // Apply the rotation to the turret
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
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
        float angleDifference = Mathf.DeltaAngle(transform.localEulerAngles.z, targetLocalAngle);
        Debug.Log("angle Difference: " + angleDifference);
        if (Mathf.Abs(angleDifference) > maxRotationAngle)
        {
            Debug.Log("Status 1");
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (targetLocalAngle < 0)
            {
                currentAngle -= rotationStep;
            }
            else
            {
                currentAngle += rotationStep;
            }
        }
        else if (Mathf.Abs(targetLocalAngle) > maxRotationAngle)
        {
            Debug.Log("Status 2");
            float rotationStep = rotationSpeed * Time.deltaTime;
            if (targetLocalAngle <= 0)
            {
                print("Minus");
                currentAngle -= rotationStep;
            }
            else
            {
                print("Plus");
                currentAngle += rotationStep;
            }
            currentAngle = Mathf.Clamp(currentAngle, -maxRotationAngle, maxRotationAngle);
        }
        else
        {
            Debug.Log("Status 3");
            float rotationStep = Mathf.Sign(angleDifference) * rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.Clamp(currentAngle + rotationStep, -maxRotationAngle, maxRotationAngle);
        }
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}

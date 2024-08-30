using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public ShellObject shellObject;
    private float shellDamage;
    private Vector3 targetPosition;
    private float speed;
    private float muzzleVelocity;
    private Vector3 shellStartPosition;


    private void Update()
    {
        shellDamage = shellObject.shellDamage;
    }
    public void LaunchFlat(Vector3 target, float speed)
    {
        this.targetPosition = target;
        this.speed = speed;
        StartCoroutine(FlatMovement());
    }

    IEnumerator FlatMovement()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        Disintegrate();
    }

    public void LaunchBallistic(Vector3 target, float muzzleVelocity)
    {
        this.targetPosition = target;
        this.muzzleVelocity = muzzleVelocity;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float timeToImpact = distanceToTarget / muzzleVelocity;

        StartCoroutine(BallisticMovement(timeToImpact));
    }

    IEnumerator BallisticMovement(float timeToImpact)
    {
        Vector3 startPosition = transform.position;
        Vector3 midPoint = (startPosition + targetPosition) / 2 + Vector3.up * (muzzleVelocity * timeToImpact * 0.5f); // Adjust height based on muzzle velocity

        float elapsedTime = 0f;
        while (elapsedTime < timeToImpact)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / timeToImpact;

            // Interpolate position using quadratic Bezier curve
            transform.position = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, targetPosition, t), t);
            yield return null;
        }
        Disintegrate();
    }


    // Destroy the shell after landing or reaching the ideal position
    void Disintegrate()
    {
        Destroy(gameObject, 2f); // Delay before destruction
    }
}

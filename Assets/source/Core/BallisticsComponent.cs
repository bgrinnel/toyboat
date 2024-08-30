using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsComponent : MonoBehaviour
{

    /// <summary>
    /// The damage radius of shot once it lands
    /// </summary>
    [SerializeField] private float AOERadius;

    /// <summary>
    /// a percentage drop off of damage for ships within the damage AOE. i.e. if ship in AOE; damage = base_dmg * pow(fall_off_percent, dist_meters)
    /// </summary>
    [Tooltip("a percentage drop off of damage for ships within the damage AOE. i.e. if ship in AOE; damage = base_dmg * pow(fall_off_percent, dist_meters)")]
    [SerializeField] private float AOEDamageFalloffPerMeter;

    /// <summary>
    /// the base damage of a direct hit
    /// </summary>
    [SerializeField] private float Damage;

    /// <summary>
    /// The velocity in a flat launch
    /// </summary>
    [SerializeField] private float velocityFlat;

    // ToDo: ballisitic logic will need to be straightened out
    /// <summary>
    /// The velocity in a ballistic launch
    /// </summary>
    [SerializeField] private float velocityComplex;

    private Vector3 targetPosition;


    public float GetVelocity(bool ballistic = false)
    {
        return ballistic ? velocityComplex : velocityFlat;
    }
    public void LaunchFlat(Vector3 target, float speed)
    {
        this.targetPosition = target;
        this.velocityFlat = speed;
        StartCoroutine(FlatMovement());
    }

    private IEnumerator FlatMovement()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, velocityFlat * Time.deltaTime);
            yield return null;
        }
        Disintegrate();
    }

    public void LaunchBallistic(Vector3 target, float muzzleVelocity)
    {
        this.targetPosition = target;
        this.velocityComplex = muzzleVelocity;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float timeToImpact = distanceToTarget / muzzleVelocity;

        StartCoroutine(BallisticMovement(timeToImpact));
    }

    private IEnumerator BallisticMovement(float timeToImpact)
    {
        Vector3 startPosition = transform.position;
        Vector3 midPoint = (startPosition + targetPosition) / 2 + Vector3.up * (velocityComplex * timeToImpact * 0.5f); // Adjust height based on muzzle velocity

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
    private void Disintegrate()
    {
        Destroy(gameObject, 2f); // Delay before destruction
    }
}

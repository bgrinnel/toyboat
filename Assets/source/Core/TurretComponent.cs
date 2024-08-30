using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretComponent: MonoBehaviour
{ 
    /// <summary>
    /// The range in meters the turret can fire
    /// </summary>
    [Tooltip("Currently not a part of the targeting calc")]
    [SerializeField] private float FireRangeMeters;

    /// <summary>
    /// How far the turret can rotate from its forward direction (set to 180 if you want the turrets to be able to rotate freely)
    /// </summary>
    [Tooltip("How far the turret can rotate from its forward direction (set to 180 if you want the turrets to be able to rotate freely)")]
    [Range(0,180)]
    [SerializeField] private float fireRotationDegrees;
    
    /// <summary>
    /// The speed the turret rotates in degrees per second
    /// </summary>
    [SerializeField] private float fireRotationSpeed;

    /// <summary>
    /// The time in seconds it takes for this turret to reload once it has fired
    /// </summary>
    [SerializeField] private float fireRateSeconds;

    /// <summary>
    /// whether this turret used Ballistics or flat firing
    /// </summary>
    [SerializeField] private bool bComplexFiring = false;


    public float muzzleVelocity; // Muzzle velocity for ballistic trajectory
    public GameObject BallisticsVariant;

    private bool bReloading = false;

    public void Fire(Vector3 targetPosition)
    {
        if (bReloading)
        {
            return;
        }

        if (!bComplexFiring)
        {
            FireFlat(targetPosition);
        }

        else
        {
            FireComplex(targetPosition);
        }

        StartCoroutine(Reload());
    }
    // Flat trajectory firing
    public void FireFlat(Vector3 targetPosition)
    {
        GameObject shell_obj = Instantiate(BallisticsVariant, transform.position, transform.rotation);
        BallisticsComponent shell = shell_obj.GetComponent<BallisticsComponent>();
        shell.LaunchFlat(targetPosition, shell.GetVelocity());
    }

    // Ballistic trajectory firing
    public void FireComplex(Vector3 targetPosition)
    {
        GameObject shell = Instantiate(BallisticsVariant, transform.position, transform.rotation);
        shell.GetComponent<Shell>().LaunchBallistic(targetPosition, muzzleVelocity);
    }

    private IEnumerator Reload()
    {
        bReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(fireRateSeconds); // Wait for the reload time to complete
        bReloading = false;
        Debug.Log("Reloaded!");
    }

    public bool IsReloading() {
        return bReloading;
    } 

    public float RotationDegrees() 
    {
        return fireRotationDegrees;
    }

    public float RotationSpeed()
    {
        return fireRotationSpeed;
    }
}

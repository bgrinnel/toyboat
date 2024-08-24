using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool Ballistic = false;

    public GunObject gunObject; // Reference to the GunObject scriptable 

    // Flat trajectory firing
    public void FireFlat(Vector3 targetPosition, bool AP)
    {
        GameObject shell = Instantiate(gunObject.shellPrefab, transform.position, transform.rotation);
        shell.GetComponent<Shell>().LaunchFlat(targetPosition, gunObject.flatShellSpeed);
    }

    // Ballistic trajectory firing
    public void FireBallistic(Vector3 targetPosition, float timeToImpact, bool AP)
    {
        GameObject shell = Instantiate(gunObject.shellPrefab, transform.position, transform.rotation);
        shell.GetComponent<Shell>().LaunchBallistic(targetPosition, timeToImpact, gunObject.muzzleVelocity);
    }
}

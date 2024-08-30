using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool Ballistic = false;

    public GunObject gunObject; // Reference to the GunObject scriptable 

    private bool ballistic = false;

    private bool reloading;

    public void Fire(Vector3 targetPosition)
    {
        if (reloading)
        {
            return;
        }

        if (!ballistic)
        {
            FireFlat(targetPosition);
        }

        else
        {
            FireBallistic(targetPosition);
        }

        StartCoroutine(Reload());
    }
    // Flat trajectory firing
    public void FireFlat(Vector3 targetPosition)
    {
        GameObject shell = Instantiate(gunObject.shellPrefab, transform.position, transform.rotation);
        shell.GetComponent<Shell>().LaunchFlat(targetPosition, gunObject.flatShellSpeed);
    }

    // Ballistic trajectory firing
    public void FireBallistic(Vector3 targetPosition)
    {
        GameObject shell = Instantiate(gunObject.shellPrefab, transform.position, transform.rotation);
        shell.GetComponent<Shell>().LaunchBallistic(targetPosition, gunObject.muzzleVelocity);
    }

    private IEnumerator Reload()
    {
        reloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(gunObject.gunReload); // Wait for the reload time to complete
        reloading = false;
        Debug.Log("Reloaded!");
    }
}

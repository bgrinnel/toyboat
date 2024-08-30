using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretComponent: MonoBehaviour
{ 
    /// <summary>
    /// The base damage of a direct hit from the fire
    /// </summary>
    [SerializeField] private int FireDamage;

    /// <summary>
    /// The range in meters the turret can fire
    /// </summary>
    [SerializeField] private float FireRangeMeters;

    /// <summary>
    /// The damage radius of shot once it lands
    /// </summary>
    [SerializeField] private float FireAOERadius;

    /// <summary>
    /// a percentage drop off of damage for ships within the damage AOE. i.e. if ship in AOE; damage = base_dmg * pow(fall_off_percent, dist_meters)
    /// </summary>
    [Tooltip("a percentage drop off of damage for ships within the damage AOE. i.e. if ship in AOE; damage = base_dmg * pow(fall_off_percent, dist_meters)")]
    [SerializeField] private float FireAOEDamageFalloffPerMeter;

    /// <summary>
    /// How far the turret can rotate from its forward direction (set to 180 if you want the turrets to be able to rotate freely)
    /// </summary>
    [Tooltip("How far the turret can rotate from its forward direction (set to 180 if you want the turrets to be able to rotate freely)")]
    [SerializeField] private float FireRoatationDegrees;

    /// <summary>
    /// The time in seconds it takes for this turret to reload once it has fired
    /// </summary>
    [SerializeField] private float FireRateSeconds;

    // Start is called before the first frame updates
    void Start()
    {
        // not sure there is anything to do in this
    }

    // Update is called once per frame
    void Update()
    {
        // do turrent related updates that are detached from the centralized logic of the boat
    }
}

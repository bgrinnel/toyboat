using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Follow_Script : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private unit_movement move_Script;
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        
       transform.position = Vector3.MoveTowards(transform.position, target.transform.position, .3f); 
       transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotationSpeed * Time.deltaTime);
    }
    public void PassDestination(Vector3 destPoint, bool shiftPressed){
        move_Script.updateDestinationList(destPoint, shiftPressed);
    }
}

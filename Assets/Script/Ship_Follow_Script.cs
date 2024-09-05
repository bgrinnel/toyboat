using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Follow_Script : MonoBehaviour
{


//could do gotoPoint instead, with first halfturn and second moveTowards
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject wakeLeft;
    [SerializeField] private GameObject wakeRight;
    [SerializeField] private Transform parentship;
    List<Vector3> destinationList = new List<Vector3>();
    [SerializeField] private unit_movement move_Script;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    

    // Update is called once per frame
    void Update()
    {
        
        if(targetPos == Vector3.zero && destinationList.Count !=0){
            targetPos = destinationList[0];
            destinationList.RemoveAt(0);
        }
        
        float speedTick = Time.deltaTime * speed;
        Vector3 targetDir = new Vector3(targetPos.x - parentship.position.x, parentship.position.y, targetPos.z - parentship.position.z);

        if(Quaternion.LookRotation(targetDir) != Quaternion.identity && targetPos != Vector3.zero){
            //Debug.Log(targetDir);
            Quaternion lookDir = Quaternion.LookRotation(targetDir,Vector3.up);
            parentship.rotation =  Quaternion.RotateTowards(parentship.rotation, lookDir, (rotSpeed * Time.deltaTime));
            parentship.localEulerAngles = new Vector3(0, parentship.localEulerAngles.y, parentship.localEulerAngles.z);
            parentship.position += transform.forward* speedTick;
            GameObject wakeL = Instantiate(wakeLeft, parentship);
            wakeL.transform.parent = null;
            Destroy(wakeL,2f);
            GameObject wakeR = Instantiate(wakeRight, parentship);
            wakeR.transform.parent = null;
            Destroy(wakeR,2f);
        }
        else if(targetPos != Vector3.zero){
            parentship.position = Vector3.MoveTowards(parentship.position, target.transform.position, speedTick);
            GameObject wakeL = Instantiate(wakeLeft, parentship);
            wakeL.transform.parent = null;
            Destroy(wakeL,2f);
            GameObject wakeR = Instantiate(wakeRight, parentship);
            wakeR.transform.parent = null;
            Destroy(wakeR,2f);
        }
        float dist = Vector3.Distance(targetPos, parentship.position);
        if (dist < 2f){
            targetPos = Vector3.zero;
        }
        parentship.position = new Vector3(parentship.position.x,0.8f,parentship.position.z);
        
    }
    public void PassDestination(Vector3 destPoint, bool shiftPressed){
        //destPoint = new Vector3(destPoint.x,0.5f,destPoint.z);
       if(shiftPressed){
            destinationList.Add(destPoint);
        }
        else{
            destinationList.Clear();
            targetPos = destPoint;
        }
    }
    /*
    //stack overflow code
    public void mathNav(){
        angleToP = initial_direction - 90
        P.x = Origin.x + r * cos(angleToP)
        P.y = Origin.y + r * sin(angleToP)
        dx = Destination.x - P.x
        dy = Destination.y - P.y
        h = sqrt(dx*dx + dy*dy)
        if (h < r)
            return false
            d = sqrt(h*h - r*r)
        theta = arccos(r / h)
        phi = arctan(dy / dx) [offset to the correct quadrant]
        Q.x = P.x + r * cos(phi + theta)
        Q.y = P.y + r * sin(phi + theta)
        distance = unit_speed * elapsed_time
        loop i = 0 to 3:
        if (distance < LineSegment[i].length)
            // Unit is somewhere on this line segment
            if LineSegment[i] is an arc
                //determine current angle on arc (theta) by adding or
                //subtracting (distance / r) to the starting angle
                //depending on whether turning to the left or right
                position.x = LineSegment[i].center.x + r*cos(theta)
                position.y = LineSegment[i].center.y + r*sin(theta)
            //determine current direction (direction) by adding or
            //subtracting 90 to theta, depending on left/right
            else
                position.x = LineSegment[i].start.x 
                + distance * cos(LineSegment[i].line_angle)
                position.y = LineSegment[i].start.y
                + distance * sin(LineSegment[i].line_angle)
            direction = theta
            break out of loop
        else
            distance = distance - LineSegment[i].length
    }
    */
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Follow_Script : MonoBehaviour
{


//could do gotoPoint instead, with first halfturn and second moveTowards
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject target;
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
        Vector3 targetDir = targetPos - transform.position;

        if(Quaternion.LookRotation(targetDir) != Quaternion.identity && targetPos != Vector3.zero){
            //Debug.Log(targetDir);
            Quaternion lookDir = Quaternion.LookRotation(targetDir);
            transform.rotation =  Quaternion.RotateTowards(transform.rotation, lookDir, (rotSpeed * Time.deltaTime));
            transform.position += transform.forward* speedTick;
        }
        else if(targetPos != Vector3.zero){
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speedTick); 
        }
        float dist = Vector3.Distance(targetPos, transform.position);
        if (dist < 2f){
            targetPos = Vector3.zero;
        }
        
        
    }
    public void PassDestination(Vector3 destPoint, bool shiftPressed){
        destPoint = new Vector3(destPoint.x,10,destPoint.z);
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

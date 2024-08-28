using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Follow_Script : MonoBehaviour
{


//could do gotoPoint instead, with first halfturn and second moveTowards
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private GameObject target;
    [SerializeField] private unit_movement move_Script;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    

    // Update is called once per frame
    void Update()
    {
        
        
        /*
        if (!agent.isStopped){
            transform.position += Vector3.forward* Time.deltaTime;
        }
        */
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
        
        
    }
    public void PassDestination(Vector3 destPoint, bool shiftPressed){
       // move_Script.updateDestinationList(destPoint, shiftPressed);
       targetPos = destPoint;
    }
    /*
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
    public void moveDestination(Vector3 destPoint,){
        turnAngSpeed = 0.4 //direction changing speed
        ForwordSpeed = 40 // full forward speed
        turnForwordSpeed = ForwordSpeed *0.6 // forward speed while turning
        function ent:update(dt)
            dir = getVec2(self.tx-self.x,self.ty-self.y) // ship --> target direction (vec2)
            dir = dir.normalize(dir) //normalized                               
            a= dir:angle() - self.forward:angle() //angle between target direction e current forward ship vector
            if (a<0) then
                a=a+math.pi *2 // some workaround to have all positive values
            end
            
            if a > 0.05 then // if angle difference 
                if a < math.pi then
                    //turn right
                    self.forward = vec2.rotate(self.forward,getVec2(0,0),turnAngSpeed * dt)
                else
                    //turn left
                    self.forward = vec2.rotate(self.forward,getVec2(0,0),-turnAngSpeed * dt)
                end             
                //apply turnForwordSpeed
                self.x = self.x+ self.forward.x * turnForwordSpeed * dt
                self.y = self.y+ self.forward.y * turnForwordSpeed * dt
            else 
                //applly ForwordSpeed
                self.x = self.x+ self.forward.x * ForwordSpeed * dt
                self.y = self.y+ self.forward.y * ForwordSpeed * dt
            end
end
    }
    */
}

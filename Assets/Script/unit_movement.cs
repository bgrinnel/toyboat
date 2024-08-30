using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class unit_movement : MonoBehaviour
{

    List<Vector3> destinationList = new List<Vector3>();
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private LayerMask background;
    
    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance && destinationList.Count >0){
            updatePath();
        }
    }
    public void updateDestinationList(Vector3 destPoint, bool shiftPressed){
        if(shiftPressed){
            destinationList.Add(destPoint);
        }
        else{
            destinationList.Clear();
            destinationList.Add(destPoint);
            updatePath();
        }
    }
    public void updatePath(){

        agent.destination = destinationList[0];
        destinationList.RemoveAt(0);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class unit_movement : MonoBehaviour
{
    Vector3 screenPos;
    [SerializeField] NavMeshAgent agent;
    public LayerMask background;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
        
            screenPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if(Physics.Raycast(ray,  out RaycastHit hitData, 100, background)){
                agent.destination = hitData.point;
            }
        }
    }
}

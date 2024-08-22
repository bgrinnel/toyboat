using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseposTest : MonoBehaviour
{
    public Vector3 screenPos;
    public Vector3 worldPos;
    public LayerMask unitLayerMask;
    public LayerMask background;

    // Update is called once per frame
    void Update()
    {
        screenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if(Physics.Raycast(ray,  out RaycastHit hitData, 100, unitLayerMask)){
            worldPos =hitData.point;
        }
        transform.position = worldPos;
    }
}

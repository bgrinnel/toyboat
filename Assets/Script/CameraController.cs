using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    [SerializeField] float scrollZoomSpeed;
    [SerializeField] float cameraSpeed;

    private void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.up * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.down * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * Time.deltaTime * cameraSpeed;
        }
        Vector3 pos = transform.position;
        if((pos.z + (Input.mouseScrollDelta.y * scrollZoomSpeed)) > -20 && (pos.z + (Input.mouseScrollDelta.y * scrollZoomSpeed)) < -1)
            pos.z += Input.mouseScrollDelta.y * scrollZoomSpeed;
        transform.position = pos;

    }
    
}

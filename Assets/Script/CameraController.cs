using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    [SerializeField] float scrollZoomSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField]
    private GameObject playUI;
    [SerializeField]
    private GameObject settingsUI;
    private bool SettingsMenu = false;

    private void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.forward * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.back * Time.deltaTime * cameraSpeed;
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * Time.deltaTime * cameraSpeed;
        }
        Vector3 pos = transform.position;
        if((pos.y - (Input.mouseScrollDelta.y * scrollZoomSpeed)) > 10 && (pos.y - (Input.mouseScrollDelta.y * scrollZoomSpeed)) < 80){
            pos.y -= Input.mouseScrollDelta.y * scrollZoomSpeed;
            pos.z += Input.mouseScrollDelta.y * scrollZoomSpeed;
        }
        transform.position = pos;
        if(Input.GetKeyDown(KeyCode.Escape)) {
            MenuChange();
        }

    }
    public void MenuChange()
    {
        if(SettingsMenu){
            playUI.SetActive(true);
            settingsUI.SetActive(false);
            SettingsMenu = false;
        }
        else{
            playUI.SetActive(false);
            settingsUI.SetActive(true);
            SettingsMenu = true;
        }
        
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public bool getSettingsMenu()
    {
        return(SettingsMenu);
    }
}

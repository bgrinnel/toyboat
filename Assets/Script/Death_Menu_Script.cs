using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    [SerializeField] private SceneAsset mainMenuScene;
    [SerializeField] private SceneAsset survivalScene;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMainMenuBtnClicked()
    {
        SceneManager.LoadScene(mainMenuScene.name);
    }

    public void OnTryAgainBtnClicked()
    {
        SceneManager.LoadScene(survivalScene.name);
    }
}

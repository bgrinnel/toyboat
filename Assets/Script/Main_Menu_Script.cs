using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject settingsMenu;

    [SerializeField] private SceneAsset survivalScene;
    public void PlayButton()
    {
        SceneManager.LoadScene(survivalScene.name);
    }
    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void SettingsBackButton()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
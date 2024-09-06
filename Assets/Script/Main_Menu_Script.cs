using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject tutorialMenu;
    [SerializeField]
    private GameObject settingsMenu;
    public void PlayButton()
    {
        SceneManager.LoadScene("navMeshTest");
    }
    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void TutorialButton()
    {
        mainMenu.SetActive(false);
        tutorialMenu.SetActive(true);
    }
    public void SettingsBackButton()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void TutorialBackButton()
    {
        mainMenu.SetActive(true);
        tutorialMenu.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
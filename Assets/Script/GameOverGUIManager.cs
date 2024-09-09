using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverGUIManager : ManagerObject<GameOverGUIManager>
{
    [SerializeField] private SceneAsset mainMenuScene;
    [SerializeField] private SceneAsset survivalScene;
    public TMP_Text titleTxt;
    public TMP_Text subTitleTxt;


    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void OnMainMenuBtnClicked()
    {
        SceneManager.LoadScene(mainMenuScene.name);
    }

    public void OnTryAgainBtnClicked()
    {
        SceneManager.LoadScene(survivalScene.name);
    }

    public void SetTitle(string title)
    {
        titleTxt.text = title;
    }

    public void SetSubTitle(string subTitle)
    {
        subTitleTxt.text = subTitle;
    }
}

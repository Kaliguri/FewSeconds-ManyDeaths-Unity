using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VFavorites.Libs;

public class GameMainMenuManager : MonoBehaviour
{
    [Title("GameObj Reference")]
    [SerializeField] GameObject content;
    [SerializeField] GameObject firstPage;
    [SerializeField] List<GameObject> contentPagesList;

    [Title("MainMenu")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string MainMenuPath;
    public string MainMenuSceneName;

    private InputActions inputActions;

    void Awake()
    {
        inputActions = new InputActions();

        inputActions.UI.OpenMenu.performed += _ => ActiveChange();
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
    
    void Start()
    {
        content.SetActive(false);
    }

    public void ActiveChange()
    {
        content.SetActive(!content.activeSelf);

        foreach (var page in contentPagesList)
        {
            page.SetActive(false);
        }
        firstPage.SetActive(true);
    }

    public void GoMainMenu()
    {
        PlayerInfoData.instance.transform.parent.gameObject.Destroy();
        SceneLoaderManager.instance.LoadScene(MainMenuSceneName, true);
        
    }

    public void RestartScene()
    {
        SceneLoaderManager.instance.RestartScene();
    }
    
    

}

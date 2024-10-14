using System.Collections.Generic;
using Sirenix.OdinInspector;
using Steamworks;
using Unity.Netcode;
using Steamworks.Data;
using UnityEngine;

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
        Destroy(PlayerInfoData.instance.transform.parent.gameObject);
        LobbySaver.instance.currentLobby?.Leave();
        LobbySaver.instance.currentLobby = null;
        NetworkManager.Singleton.Shutdown();
        SceneLoaderManager.instance.LoadScene(MainMenuSceneName, false);
        
    }

    public void RestartScene()
    {
        SceneLoaderManager.instance.RestartScene();
    }
    
    

}

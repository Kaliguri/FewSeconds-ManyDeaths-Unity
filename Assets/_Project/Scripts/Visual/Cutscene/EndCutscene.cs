using Sirenix.OdinInspector;
using UnityEngine;
using Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Sonity;

public class EndCutscene : MonoBehaviour
{
    [Title("Scene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string ScenePath;
    [SerializeField] string NextScene;

    [Title("Settings")]
    [SerializeField] bool GoNextScene = true;

    [Title("Other")]
    
    private SceneLoaderManager sceneLoaderManager => FindObjectOfType<SceneLoaderManager>();
    private MainMenuSceneContainer sceneManager => FindObjectOfType<MainMenuSceneContainer>();
    private InputActions inputActions;

    void Awake()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            inputActions = new InputActions();
            inputActions.UI.SkipCutscene.performed += _ => NextSceneLoader();
            inputActions.Enable();
        }
    }

    void OnDisable()
    {
        if (NetworkManager.Singleton.IsHost) inputActions.Disable();
    }

    public void NextSceneLoader()
    {
        if (GoNextScene)
        {
            sceneLoaderManager.LoadScene(NextScene, true);
        }
    }
}

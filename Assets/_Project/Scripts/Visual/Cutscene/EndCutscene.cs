using Sirenix.OdinInspector;
using UnityEngine;
using Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class EndCutscene : MonoBehaviour
{
    [Title("Scene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string ScenePath;
    [SerializeField] string NextScene;

    [Title("Settings")]
    [SerializeField] bool GoNextScene = true;

    private SceneLoaderManager sceneManager => FindObjectOfType<SceneLoaderManager>();
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

    public void NextSceneLoader()
    {
        if (GoNextScene)
        {
            sceneManager.LoadScene(NextScene, true);
        }
    }
}

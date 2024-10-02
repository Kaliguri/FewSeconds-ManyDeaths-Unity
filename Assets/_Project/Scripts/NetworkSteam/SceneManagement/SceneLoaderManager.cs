using System.Collections;
using Sirenix.OdinInspector;
using Unity.Netcode;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sonity;

public class SceneLoaderManager : MonoBehaviour
{
    [Title("LoadingScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string LoadingScenePath;
    public string LoadingScene;

    [Title("MainMenuScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string MainMenuScenePath;
    public string MainMenuScene;

    [Title("FirstSessionScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string FirstSessionScenePath;
    public string FirstSessionScene;
    
    [Title("TrainingScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string TrainingScenePath;
    public string TrainingScene;

    private SoundManager music => FindObjectOfType<SoundManager>();

    private Scene m_LoadedScene;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == LoadingScene)
        { StartCoroutine(LoadMainScene()); }
    }

    public IEnumerator LoadMainScene()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        LoadScene(MainMenuScene, false);
    }

    public void LoadFistSessionScene()
    {
        LoadScene(FirstSessionScene, true);
    }

    public void LoadTrainingScene()
    {
        LoadScene(MainMenuScene, false);
    }

    public void LoadScene(string sceneName, bool IsNetworkScene)
    {
        OffMusic();

        if (IsNetworkScene)
        {
            if (NetworkManager.Singleton.IsHost && !string.IsNullOrEmpty(sceneName))
            {
                SceneLoaderWrapperLocal.Instance.LoadScene(sceneName, useNetworkSceneManager: IsNetworkScene);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneLoaderWrapperLocal.Instance.LoadScene(sceneName, useNetworkSceneManager: IsNetworkScene);
            }
        }
    }

    [Button("Off Music")]
    public void OffMusic()
    {
        music.StopEverything(false);
    }

}

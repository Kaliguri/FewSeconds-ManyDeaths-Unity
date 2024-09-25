using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : NetworkBehaviour
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

    private string m_SceneName;
    private Scene m_LoadedScene;

    public bool SceneIsLoaded
    {
        get
        {
            if (m_LoadedScene.IsValid() && m_LoadedScene.isLoaded)
            {
                return true;
            }
            return false;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        }

        base.OnNetworkSpawn();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == LoadingScene)
        { StartCoroutine(LoadMainScene()); }
    }

    public IEnumerator LoadMainScene()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        SceneManager.LoadScene(MainMenuScene);
    }

    public void LoadFistSessionScene()
    {
        //if (NetworkManager.Singleton.IsHost) NetworkManager.Singleton.SceneManager.LoadScene(FirstSessionScene, LoadSceneMode.Single);
        LoadScene(FirstSessionScene);
    }

    public void LoadTrainingScene()
    {
        SceneManager.LoadScene(TrainingScene, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsHost && !string.IsNullOrEmpty(sceneName))
        {
            var status = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            CheckStatus(status);
        }
    }

    private void CheckStatus(SceneEventProgressStatus status, bool isLoading = true)
    {
        var sceneEventAction = isLoading ? "load" : "unload";
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning($"Failed to {sceneEventAction} {m_SceneName} with" +
                $" a {nameof(SceneEventProgressStatus)}: {status}");
        }
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    // We want to handle this for only the server-side
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        // *** IMPORTANT ***
                        // Keep track of the loaded scene, you need this to unload it
                        m_LoadedScene = sceneEvent.Scene;
                    }
                    Debug.Log($"Loaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.UnloadComplete:
                {
                    Debug.Log($"Unloaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
            case SceneEventType.LoadEventCompleted:
            case SceneEventType.UnloadEventCompleted:
                {
                    var loadUnload = sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted ? "Load" : "Unload";
                    Debug.Log($"{loadUnload} event completed for the following client " +
                        $"identifiers:({sceneEvent.ClientsThatCompleted})");
                    if (sceneEvent.ClientsThatTimedOut.Count > 0)
                    {
                        Debug.LogWarning($"{loadUnload} event timed out for the following client " +
                            $"identifiers:({sceneEvent.ClientsThatTimedOut})");
                    }
                    break;
                }
        }
    }

    public void UnloadScene()
    {
        // Assure only the server calls this when the NetworkObject is
        // spawned and the scene is loaded.
        if (!NetworkManager.Singleton.IsHost || !IsSpawned || !m_LoadedScene.IsValid() || !m_LoadedScene.isLoaded)
        {
            return;
        }

        // Unload the scene
        var status = NetworkManager.Singleton.SceneManager.UnloadScene(m_LoadedScene);
        CheckStatus(status, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    [Title("MainMenuScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string MainMenuScenePath;
    public string MainMenuScene;

    private SceneLoaderManager sceneLoaderManager => FindObjectOfType<SceneLoaderManager>();

    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    public IEnumerator LoadMainScene()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        sceneLoaderManager.LoadScene(MainMenuScene, false);
    }
}

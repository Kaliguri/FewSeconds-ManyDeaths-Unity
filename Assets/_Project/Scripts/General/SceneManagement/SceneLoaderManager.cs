using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    private SoundManager soundManager => FindObjectOfType<SoundManager>();

    public static SceneLoaderManager instance;
    void Awake()
    {
        instance = this;
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

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name, true);
    }

    [Button("Off Music")]
    public void OffMusic()
    {
        soundManager.StopEverything(false);
    }
}

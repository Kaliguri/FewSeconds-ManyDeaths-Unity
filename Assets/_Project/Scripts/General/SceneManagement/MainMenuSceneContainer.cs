using System.Collections;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sonity;

public class MainMenuSceneContainer : MonoBehaviour
{

    [Title("FirstSessionScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string FirstSessionScenePath;
    public string FirstSessionScene;
    
    [Title("TrainingScene")]
    [FilePath(ParentFolder = "Assets/_Project/Scenes/InGame")]
    [SerializeField] string TrainingScenePath;
    public string TrainingScene;

    private SceneLoaderManager sceneLoaderManager => FindObjectOfType<SceneLoaderManager>();

    public void LoadFistSessionScene()
    {
        sceneLoaderManager.LoadScene(FirstSessionScene, true);
    }

    public void LoadTrainingScene()
    {
        sceneLoaderManager.LoadScene(TrainingScene, false);
    }

}

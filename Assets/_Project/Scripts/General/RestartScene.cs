using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private InputActions inputActions;
    void Awake()
    {
        inputActions = new InputActions();
        inputActions.UI.RestartScene.performed += _ => ReastartLevel();
        inputActions.Enable();
    }
    public void ReastartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

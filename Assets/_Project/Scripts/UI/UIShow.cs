using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIShow : MonoBehaviour 
{

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
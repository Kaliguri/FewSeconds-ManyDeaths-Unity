using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    [Title("Current Page")]
    [SerializeField] GameObject currentPage;


    [Title("Guide Pages")]
    [SerializeField] List<GameObject> guidePagesList = new();

    void Start()
    {
        SetActivePage();
    }

    void SetActivePage()
    {        
        foreach (var page in guidePagesList)
        {
            if (page == currentPage) page.SetActive(true);
            else page.SetActive(false); 
        }
    }

    public void Select(int number)
    {
        currentPage.SetActive(false);

        currentPage = guidePagesList[number];

        currentPage.SetActive(true);

        //Debug.Log("Select Page!");
    }
}

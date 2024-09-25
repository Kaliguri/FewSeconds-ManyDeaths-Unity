using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDataBase : MonoBehaviour
{
    [Header("UI GameObj Ref")]
    [SerializeField] GameObject EndTurnButton;
    [SerializeField] GameObject Timer;

    [SerializeField] List<GameObject> SkillButtonList;


    public void EndTurnButtonActiveChange(bool ActiveBool)
    {
        EndTurnButton.SetActive(ActiveBool);
        //Debug.Log("EndTurnButton.SetActive " + ActiveBool);
    }

    public void TimerActiveChange(bool ActiveBool)
    {
        Timer.SetActive(ActiveBool);
        //Debug.Log("TimerActiveChange.SetActive " + ActiveBool);
    }
    public void SkillButtonListActiveChange(bool ActiveBool)
    {
        foreach (GameObject obj in SkillButtonList) 
        { obj.SetActive(ActiveBool);}
        //Debug.Log("SkillButtonListActiveChange.SetActive " + ActiveBool);
    }

}

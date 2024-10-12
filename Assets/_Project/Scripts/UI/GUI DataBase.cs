using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDataBase : MonoBehaviour
{
    [Header("UI GameObj Ref")]
    [SerializeField] GameObject EndTurnButton;
    [SerializeField] GameObject Timer;

    [SerializeField] List<GameObject> SkillButtonList;

    [Header("Parameters")]
    [SerializeField] float alfhaParameter = 0.5f;


    public void EndTurnButtonActiveChange(bool ActiveBool)
    {
        if (ActiveBool)
        {
            EndTurnButton.GetComponent<Button>().enabled = true;
            Color color = EndTurnButton.GetComponent<Image>().color;
            EndTurnButton.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
        }
        else
        {
            EndTurnButton.GetComponent<Button>().enabled = false;
            Color color = EndTurnButton.GetComponent<Image>().color;
            EndTurnButton.GetComponent<Image>().color = new Color(color.r, color.g, color.b, alfhaParameter);
        }
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

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class QuestManagerUI : MonoBehaviour
{
    [Header ("Data")]
    public QuestData Data;

    [Header("UI Reference")]

    [Header("Text")]
    public LocalizeStringEvent NameObj;
    public LocalizeStringEvent DescriptionObj;

    [Header("Button")]
    public List<LocalizeStringEvent> ButtonChoiceList;

    private QuestResultScript script => Data.Script;

    public void Start() { DataTransfer(); }
    public void DataTransfer()
    {
        TextDataTransfer();
    }

    public void TextDataTransfer()
    {
        NameObj.StringReference = Data.Name;
        DescriptionObj.StringReference = Data.Description;

        for (int i = 0; i < Data.ChoiceList.Count; i++)
        {
            ButtonChoiceList[i].StringReference = Data.ChoiceList[i]; 
        }
    }

    public void Select(int id) 
    {
        script.Cast(id);
    }

}

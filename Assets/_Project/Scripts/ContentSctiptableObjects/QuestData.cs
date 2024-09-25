using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "QuestData", menuName = "FewSecondsManyDeaths/QuestData")]
public class QuestData : ScriptableObject
{

    [Header("Text")]
    public LocalizedString Name;
    public LocalizedString Description;
    
    [Header("Choice")]
    public List<LocalizedString> ChoiceList;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public QuestResultScript Script;

} 

/*
[Serializable]
public class Choice
{
    public LocalizedString ChoiceDescriptionList;
    public int IDScript;

}

*/
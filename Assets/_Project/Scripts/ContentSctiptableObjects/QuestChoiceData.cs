using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestChoiceData", menuName = "FewSecondsManyDeaths/Legacy/QuestChoiceData")]
public class QuestChoiceData : ScriptableObject
{
    [Header("General")]
    [TextArea] public string Description;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public QuestResultScript ResultQuestScript = new QuestResultScript();
}

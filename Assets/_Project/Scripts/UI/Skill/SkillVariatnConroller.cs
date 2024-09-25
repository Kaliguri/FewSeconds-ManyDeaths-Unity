using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillVariantConroller : MonoBehaviour
{
    public List<GameObject> PrefabForSkillVariantList;

    public int SkillNumber;

    void Start()
    {
        SkillNumberTransferInPrefabs();
    }

    void SkillNumberTransferInPrefabs()
    {
        foreach (var prefab in PrefabForSkillVariantList)
        {
            prefab.GetComponent<SkillChoiceButtonController>().SkillNumber = SkillNumber;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillChoicePanelConrtoller : MonoBehaviour
{
    public List<GameObject> SkillVariantPanelList;
    private List<List<GameObject>> AllPrefabSkillVariantList = new();

    private PlayerInfoData PlayerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private List<SkillVariations> SkillList => PlayerInfoData.HeroDataList[PlayerInfoData.PlayerIDThisPlayer].SkillList;
    void Awake()
    {
        GlobalEventSystem.HeroChanged.AddListener(DataTranfer);
    }

    void Start()
    {
        FillList();
        DataTranfer();
    }

    void FillList()
    {
        foreach (GameObject skillVariant in SkillVariantPanelList)
        AllPrefabSkillVariantList.Add(skillVariant.GetComponent<SkillVariantConroller>().PrefabForSkillVariantList);
        
    }
    public void DataTranfer()
    {
        for (int SkillVariantNumber = 0; SkillVariantNumber < AllPrefabSkillVariantList.Count; SkillVariantNumber++)
        for (int PrefabNumber = 0; PrefabNumber < AllPrefabSkillVariantList[SkillVariantNumber].Count; PrefabNumber++)
        {
            var Prefab = AllPrefabSkillVariantList[SkillVariantNumber][PrefabNumber];
            var Data = Prefab.GetComponent<SkillChoiceButtonController>();

            if (SkillVariantNumber < SkillList.Count && PrefabNumber < SkillList[SkillVariantNumber].SkillVariationsList.Count )
            {  
                Prefab.SetActive(true);
                DataTransferInPrefab(Data, SkillList[SkillVariantNumber].SkillVariationsList[PrefabNumber]);
            }
            else Prefab.SetActive(false);
        }
    }
    void DataTransferInPrefab(SkillChoiceButtonController data, SkillData skillData)
    {
        data.IconObj.sprite = skillData.SkillIcon;
        data.NameObj.StringReference = skillData.Name;
        data.DescriptionObj.StringReference = skillData.Description;
    }

}

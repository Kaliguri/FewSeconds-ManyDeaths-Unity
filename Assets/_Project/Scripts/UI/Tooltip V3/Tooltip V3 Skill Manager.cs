using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TooltipV3SkillManager : TooltipV3ParentManager
{
    [PropertySpace(SpaceBefore = 25)]

    [Title("Tooltip V3 Skill")]

    [Title("LocalizeStringEvent Reference")]
    [SerializeField] LocalizeStringEvent SkillName;
    [SerializeField] LocalizeStringEvent EnergyCost;
    [SerializeField] LocalizeStringEvent Cooldown;
    [SerializeField] LocalizeStringEvent Description;
    [SerializeField] LocalizeStringEvent NarrativeDescription;

    [Title("GameObject Reference")]
    [SerializeField] Image Icon;

    [Title("Local Variables")]
    public int EnergyCostValue = 0;
    public int CooldownValue = 0;

    #region TooltipV3 ParentMethods

    public void OnDestroy() { HideTooltip();}
    public void OnDisable() { HideTooltip();}

    new void Awake()
    {
        TooltipContentShow();
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    new void LocalizeStringEventListInizizalize()
    {
        LocalizeStringEventList = new List<LocalizeStringEvent>
        {
            SkillName,
            Description,
            NarrativeDescription,
            EnergyCost,
            Cooldown
        };
    }

    #endregion
    #region NewMethods


    public void SkillDataTransfer(SkillData skillData)
    {
        SkillName.StringReference = skillData.Name;
        Description.StringReference = skillData.Description;

        Icon.sprite = skillData.SkillIcon;
        
        EnergyCostValue = skillData.SkillScript.EnergyCost;
        CooldownValue = skillData.SkillScript.SkillCooldown;
    }

    #endregion

}


    /*
    void UpdateTextSize()
    {
        foreach (var item in TextItemList)
        {
            var textItem = item.GetComponent<TextMeshProUGUI>();
            var rectItem = item.GetComponent<RectTransform>();

            textItem.ForceMeshUpdate();
            rectItem.sizeDelta = new Vector2(rectItem.sizeDelta.x , textItem.GetRenderedValues(true).y);
        }
    }
    */
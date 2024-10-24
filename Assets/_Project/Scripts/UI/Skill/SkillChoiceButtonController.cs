using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SkillChoiceButtonController : UISkillV3
{
    [Title("SkillChoiceButton")]
    public int SkillNumber;
    public int VariationSkillNumber;
    public new SkillData skillData;

    void Awake()
    {

    }

    public override void DataTransfer()
    {
        IconObj.sprite = skillData.SkillIcon;
        
        tooltipManager.SkillDataTransfer(skillData);
    }

    
    public void SkillChoice()
    {
        //Debug.Log(playerInfoData.SkillChoiceList.Count);
        //Debug.Log(playerInfoData.SkillChoiceList[playerID].Count);

        if (SkillNumber <= 3)
        {
        playerInfoData.SkillChoiceList[playerID].variationList[SkillNumber] = VariationSkillNumber;
        GlobalEventSystem.SendSkillChanged();
        GlobalEventSystem.SendPlayerChoiceActionUpdate(SkillNumber, playerID, VariationSkillNumber);
        }
    }

}

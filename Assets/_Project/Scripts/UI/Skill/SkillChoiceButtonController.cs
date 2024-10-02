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

    public override void DataTransfer()
    {
        IconObj.sprite = skillData.SkillIcon;
        tooltipManager.SkillName.StringReference = skillData.Name;
        tooltipManager.Description.StringReference = skillData.Description;
    }

    
    public void SkillChoice()
    {
        //Debug.Log(playerInfoData.SkillChoiceList.Count);
        //Debug.Log(playerInfoData.SkillChoiceList[playerID].Count);

        playerInfoData.SkillChoiceList[playerID].variationList[SkillNumber] = VariationSkillNumber;
        GlobalEventSystem.SendSkillChanged();
        GlobalEventSystem.SendPlayerChoiceActionUpdate(SkillNumber, playerID, VariationSkillNumber);
    }

}

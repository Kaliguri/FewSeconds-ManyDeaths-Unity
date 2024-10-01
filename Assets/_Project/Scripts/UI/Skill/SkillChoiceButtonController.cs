using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceButtonController : MonoBehaviour
{
    [Header("GameObj Reference")]
    public Image IconObj;
    [SerializeField] TooltipV3SkillManager tooltipManager;

    [Header("Other")]
    public int SkillNumber;
    public int VariationSkillNumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;

    public void DataTransferInPrefab(SkillData skillData)
    {
        IconObj.sprite = skillData.SkillIcon;
        tooltipManager.Header.StringReference = skillData.Name;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCooldownManager : MonoBehaviour
{
    [SerializeField] private List<SkillCooldownList> playersSkillCooldownLists = new();

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();

    private void Awake()
    {
        GlobalEventSystem.PredictionStageEnded.AddListener(SkillCooldownDecrease);
    }

    private void Start()
    {
        for (int i = 0; i < playerInfoData.PlayerCount; i++)
        {
            playersSkillCooldownLists.Add(new SkillCooldownList());
        }
    }

    public int GetSkillCooldown(int playerId, int skillId)
    {
        return playersSkillCooldownLists[playerId].skillsCooldownList[skillId];
    }

    public void SetSkillCooldown(int playerId, int skillId, int skillCooldown)
    {
        playersSkillCooldownLists[playerId].skillsCooldownList[skillId] = skillCooldown;
        GlobalEventSystem.SendUpdateCooldown();
    }

    private void SkillCooldownDecrease()
    {
        for (int i = 0; i < playersSkillCooldownLists.Count; i++)
        {
            for (int j = 0; j < playersSkillCooldownLists[i].skillsCooldownList.Count; j++)
            {
                if (playersSkillCooldownLists[i].skillsCooldownList[j] > 0) 
                {
                    playersSkillCooldownLists[i].skillsCooldownList[j]--;
                    
                };
            }
        }

        GlobalEventSystem.SendUpdateCooldown();
    }
}

[Serializable]
public class SkillCooldownList
{
    public List<int> skillsCooldownList = new();

    public SkillCooldownList()
    {
        skillsCooldownList = new List<int> { 0, 0, 0, 0 };
    }
}
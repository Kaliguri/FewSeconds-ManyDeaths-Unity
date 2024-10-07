using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosedSkill : MonoBehaviour
{
    private int skillId => GetComponentInParent<UISkillV3>().UINumber; 

    private void Awake()
    {
        GlobalEventSystem.PlayerSkillChoosed.AddListener(PlayerSkillChoosed);
        GlobalEventSystem.PlayerSkillUnchoosed.AddListener(PlayerSkillUnchoosed);
        GlobalEventSystem.PlayerSkillAproved.AddListener(PlayerSkillUnchoosed);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void PlayerSkillChoosed(int skillID)
    {
        if (skillId == skillID)
        {
            gameObject.SetActive(true);
        }
    }

    private void PlayerSkillUnchoosed(int skillID)
    {
        gameObject.SetActive(false);
    }
}

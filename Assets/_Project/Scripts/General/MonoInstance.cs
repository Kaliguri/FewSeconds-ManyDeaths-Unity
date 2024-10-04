using Sirenix.OdinInspector.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInstance : MonoBehaviour
{
    public static MonoInstance instance;

    private void Start()
    {
        instance = this;
    }

    public void CastEndPart2() 
    {
        GlobalEventSystem.SendPlayerSkillEnd();
    }
}
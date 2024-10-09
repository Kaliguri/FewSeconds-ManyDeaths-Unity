using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInstance : MonoBehaviour
{
    public static MonoInstance instance;

    void Awake()
    {
        if (instance == null) {instance = this;}
    }


    public void CastEndPart2() 
    {
        GlobalEventSystem.SendPlayerSkillEnd();
    }
}
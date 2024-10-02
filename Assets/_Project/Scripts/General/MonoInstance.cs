using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoInstance : MonoBehaviour
{
    public static MonoInstance instance;

    private void Start()
    {
        MonoInstance.instance = this;
    }

    public void CastEndPart2() 
    {
        GlobalEventSystem.SendPlayerActionEnd();
    }
}
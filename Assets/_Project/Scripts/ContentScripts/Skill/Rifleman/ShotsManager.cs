using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotsManager : MonoBehaviour
{
    public int maxBulletCount =  6;
    private List<int> currentBulletCountList = new();

    private void Awake()
    {
        for (int i = 0; i < PlayerInfoData.instance.PlayerCount; i++)
        {
            currentBulletCountList.Add(maxBulletCount);
        }
    }

    public int GetShotsCount(int playerID)
    {
        return currentBulletCountList[playerID];
    }

    public void SetShotsCount(int playerID, int newCount)
    {
        if (newCount <= 3) currentBulletCountList[playerID] = newCount;
        else if (newCount >= 0) currentBulletCountList[playerID] = 3;
        else currentBulletCountList[playerID] = 0;
    }
}

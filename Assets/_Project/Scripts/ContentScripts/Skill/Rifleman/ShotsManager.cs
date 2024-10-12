using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotsManager : MonoBehaviour
{
    public int maxBulletCount =  6;
    private List<int> currentBulletCountList = new();

    [SerializeField] List<GameObject> bulletInfoList;
    [SerializeField] HeroData riflemanHeroData;

    private void Awake()
    {
        foreach (var info in bulletInfoList)
        {
            info.SetActive(false);
        }

        var playerData = PlayerInfoData.instance;

        for (int i = 0; i < playerData.PlayerCount; i++)
        {
            currentBulletCountList.Add(maxBulletCount);
            if (playerData.HeroDataList[i] == riflemanHeroData)  bulletInfoList[i].SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotsManager : MonoBehaviour
{
    List<int> shotsCountList = new List<int> { 3, 3, 3, 3 };

    public int GetShotsCount(int playerID)
    {
        return shotsCountList[playerID];
    }

    public void SetShotsCount(int playerID, int newCount)
    {
        if (newCount <= 3) shotsCountList[playerID] = newCount;
        else if (newCount >= 0) shotsCountList[playerID] = 3;
        else shotsCountList[playerID] = 0;
    }
}

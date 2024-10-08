using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotsManager : MonoBehaviour
{
    [SerializeField] private List<int> maxShotsCountList = new List<int> { 6, 6, 6, 6 };
    private List<int> shotsCountList = new();

    private void Awake()
    {
        for (int i = 0; i < maxShotsCountList.Count; i++)
        {
            shotsCountList.Add(maxShotsCountList[i]);
        }
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    [SerializeField] private AvailableEntitiesData data;

    public List<HeroData> GetHeroDataList()
    {
        return data.HeroDataList;
    }
}

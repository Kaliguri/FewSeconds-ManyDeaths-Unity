using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossComboData", menuName = "FewSecondsManyDeaths/BossData/BossComboData")]
public class BossComboData : ScriptableObject
{
    [Header("General")]
    public List<BossActionData> BossActionList;
    
    [Header("Requirements")]

    public bool HeroIsNear;

}

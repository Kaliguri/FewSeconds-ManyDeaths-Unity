using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class TooltipV3BasicManager : TooltipV3ParentManager
{
    [PropertySpace(SpaceBefore = 25)]

    [Title("Tooltip V3 Basic")]


    [Title("LocalizedString")]
    public LocalizedString HeaderText;
    public LocalizedString DescriptionText;
    

    [Title("LocalizeStringEvent Reference")]
    [SerializeField] LocalizeStringEvent Header;
    [SerializeField] LocalizeStringEvent Description;

    new void Awake()
    {
        TooltipContentShow();
        DataTransfer();
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    new void LocalizeStringEventListInizizalize()
    {
        LocalizeStringEventList = new List<LocalizeStringEvent>
        {
            Header,
            Description
        };
    }

    void DataTransfer()
    {
        Header.StringReference = HeaderText;
        Description.StringReference = DescriptionText;
    }
}

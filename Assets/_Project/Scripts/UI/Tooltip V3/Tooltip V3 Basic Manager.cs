using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization.Components;

public class TooltipV3BasicManager : TooltipV3ParentManager
{
    [PropertySpace(SpaceBefore = 25)]

    [Title("Tooltip V3 Basic")]

    [Title("Text Localize")]
    public LocalizeStringEvent Header;
    public LocalizeStringEvent Description;

    public void OnDestroy() { HideTooltip();}
    public void OnDisable() { HideTooltip();}

    new void Awake()
    {
        TooltipContentShow();
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    new void LocalizeStringEventListInizizalize()
    {
        LocalizeStringEventList = new List<LocalizeStringEvent>
        {

        };
    }
}

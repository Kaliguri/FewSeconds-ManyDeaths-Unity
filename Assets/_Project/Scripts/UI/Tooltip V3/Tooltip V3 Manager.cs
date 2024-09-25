using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3Manager : MonoBehaviour
{
    [Title("Text Localize")]
    public LocalizeStringEvent Header;
    public LocalizeStringEvent Description;
    public LocalizeStringEvent NarrativeDescription;

    [Title("Tooltip Style")]
    public TooltipStyle Style;

}

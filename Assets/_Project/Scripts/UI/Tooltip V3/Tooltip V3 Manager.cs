using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3Manager : MonoBehaviour
{
    [Title("Text Localize")]
    public LocalizeStringEvent Header;
    public LocalizeStringEvent Description;
    public LocalizeStringEvent NarrativeDescription;

    [Title("Dynamic Background")]
    [Title("Dynamic Background Reference")]
    public RectTransform BackgroundRect;
    public RectTransform EndRectItem;
    [Title("Dynamic Background Settings")]
    public float ExtraWidthForBackground;
    public float ExtraHeightForBackground;

    [Title("Tooltip Style")]
    public TooltipStyle Style;

    void Update()
    {
        ResizeBackground();
    }

    void ResizeBackground()
    {
        СalculationDynamicRectSizeLegacy();
    }

    void СalculationDynamicRectSizeLegacy()
    {
        var width = EndRectItem.sizeDelta.x + ExtraWidthForBackground;
        var height = math.abs(EndRectItem.localPosition.y) + EndRectItem.sizeDelta.y + ExtraHeightForBackground;

        BackgroundRect.sizeDelta = new Vector2(width, height);
    }

}

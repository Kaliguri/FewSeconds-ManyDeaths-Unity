using System.Collections.Generic;
using System.Linq;
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
    public List<GameObject> TextItemList;

    [Title("Dynamic Background Settings")]
    public float ExtraWidthForBackground;
    public float ExtraHeightForBackground;

    [Title("Tooltip Style")]
    public TooltipStyle Style;
    void Update()
    {
        UpdateText();
        ResizeBackground();
    }

    void UpdateText()
    {
        foreach (var item in TextItemList)
        {
            var textItem = item.GetComponent<TextMeshProUGUI>();
            var rectItem = item.GetComponent<RectTransform>();

            textItem.ForceMeshUpdate();
            rectItem.sizeDelta = new Vector2(rectItem.sizeDelta.x , textItem.GetRenderedValues(true).y);
        }
    }

    void ResizeBackground()
    {
        var EndRectItem = TextItemList.Last().GetComponent<RectTransform>();
        var width = EndRectItem.sizeDelta.x + ExtraWidthForBackground;
        var height = math.abs(EndRectItem.localPosition.y) + EndRectItem.sizeDelta.y + ExtraHeightForBackground;

        BackgroundRect.sizeDelta = new Vector2(width, height);
    }

}

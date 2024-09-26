using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3Manager : MonoBehaviour
{
    [Title("Text Localize")]
    public LocalizeStringEvent Header;
    public LocalizeStringEvent Description;
    public LocalizeStringEvent NarrativeDescription;

    [Title("All Dynamic Background")]
    public RectTransform BackgroundRect;
    public RectTransform ScaleObject;
    public List<RectTransform> AllTextRectItems;

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
        var offsetMin = new Vector2(10000,10000);
        var offsetMax = new Vector2(-10000,-10000);

        for (int i = 0; i < AllTextRectItems.Count; i++)
        {
            var rectItem = AllTextRectItems[i];
            var scale = ScaleObject.localScale.x;
            

            var tempMin = rectItem.offsetMin * scale;
            tempMin.y += rectItem.transform.localPosition.y; 
            tempMin.x += rectItem.transform.localPosition.x; 

            if (offsetMin.x > tempMin.x) offsetMin.x = tempMin.x;
            if (offsetMin.y > tempMin.y) offsetMin.y = tempMin.y;

            var tempMax = rectItem.offsetMax * scale;
            tempMax.y += rectItem.transform.localPosition.y; 
            tempMax.x += rectItem.transform.localPosition.x; 

            if (offsetMax.x < tempMax.x) offsetMax.x = tempMax.x;
            if (offsetMax.y < tempMax.y) offsetMax.y = tempMax.y; 

            var textMeshPro = rectItem.GetComponent<TextMeshProUGUI>();
            textMeshPro.ForceMeshUpdate();
            rectItem.sizeDelta = new Vector2 (rectItem.sizeDelta.x, textMeshPro.GetRenderedValues(true).y);
        }
        Debug.Log(offsetMin + " " + offsetMax);
        BackgroundRect.offsetMin = offsetMin;
        BackgroundRect.offsetMax = offsetMax;
    }

    void СalculationDynamicRectSize()
    {

        for (int i = 0; i < AllTextRectItems.Count; i++)
        {
            var rectItem = AllTextRectItems[i];
            var scale = rectItem.localScale.x;

            var textMeshPro = rectItem.GetComponent<TextMeshProUGUI>();
            textMeshPro.ForceMeshUpdate();

            Vector2 textRect = textMeshPro.GetRenderedValues(true) * scale;

            rectItem.sizeDelta = textMeshPro.GetRenderedValues(true);

            BackgroundRect.sizeDelta = rectItem.sizeDelta;
        }

        

    }

}

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TooltipV3Manager : MonoBehaviour
{
    [Title("General")]
    public GameObject Background;
    [Title("")]
    [Title("Text Localize")]
    public LocalizeStringEvent Header;
    public LocalizeStringEvent Description;
    public LocalizeStringEvent NarrativeDescription;

    [Title("Dynamic Background")]
    [Title("Dynamic Background Reference")]
    public List<GameObject> TextItemList;

    [Title("Dynamic Background Settings")]
    public float ExtraWidthForBackground;
    public float ExtraHeightForBackground;

    [Title("Tooltip Style")]
    public TagFontStyle Style;

    private List<LocalizeStringEvent> LocalizeStringEventList;

    public void OnDestroy() { HideTooltip();}
    public void OnDisable() { HideTooltip();}

    void Start()
    {
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        LocalizeRefresh();
        UpdateTextSize();
        UpdateTagFontStyte();
        ResizeBackground();
        Debug.Log("Refresh!");
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    void UpdateTextSize()
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

        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    void UpdateTagFontStyte()
    {
        foreach (LocalizeStringEvent localizeSting in LocalizeStringEventList)
        { localizeSting.GetComponent<TextMeshProUGUI>().text = SetTagFontStyle(localizeSting.GetComponent<TextMeshProUGUI>().text, Style); }
    }

    public string SetTagFontStyle(string text, TagFontStyle style)
    {

        // Convert all tags to TMPro markup
        var styles = style.fontStyles;
        for(int i = 0; i < styles.Length; i++)
        {
            string addTags = "</b></i></u></s>";
            addTags += "<color=#" + ColorToHex(styles[i].color) + ">";
            if (styles[i].bold) addTags += "<b>";
            if (styles[i].italic) addTags += "<i>";
            if (styles[i].underline) addTags += "<u>";
            if (styles[i].strikethrough) addTags += "<s>";
            text = text.Replace(styles[i].tag, addTags);
        }

        return text;
    }

    public string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
    }

    void LocalizeStringEventListInizizalize()
    {
        LocalizeStringEventList = new List<LocalizeStringEvent>
        {
            Header,
            Description,
            NarrativeDescription
        };
    }

    void LocalizeRefresh()
    {
        foreach (LocalizeStringEvent localizeSting in LocalizeStringEventList)
        { localizeSting.RefreshString(); }
    }

}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3SkillManager : TooltipV3ParentManager
{
    [PropertySpace(SpaceBefore = 25)]

    [Title("Tooltip V3 Skill")]

    [Title("Text Localize")]
    public LocalizeStringEvent SkillName;
    public LocalizeStringEvent EnergyCost;
    public LocalizeStringEvent Cooldown;
    public LocalizeStringEvent Description;
    public LocalizeStringEvent NarrativeDescription;

    [Title("Dynamic Background")]
    [Title("Dynamic Background Reference")]
    [SerializeField] GameObject PseudoHeader;

    [Title("Dynamic Background Settings")]
    [SerializeField] float ExtraWidthForBackground;
    [SerializeField] float ExtraHeightForBackground;

    private List<LocalizeStringEvent> LocalizeStringEventList;

    public void OnDestroy() { HideTooltip();}
    public void OnDisable() { HideTooltip();}

    void Awake()
    {
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    public override void Refresh()
    {
        LocalizeRefresh();
        //UpdateTextSize();
        UpdateTagFontStyte();
        ResizeBackground();
        EdgeDetection();

        //Debug.Log("Refresh!");
    }

    public override void ShowTooltip()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public override void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    /*
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
    */

    void ResizeBackground()
    {
        var EndRectItem = PseudoHeader.GetComponent<RectTransform>();
        var width = EndRectItem.sizeDelta.x + ExtraWidthForBackground;
        var height = math.abs(EndRectItem.localPosition.y) + EndRectItem.sizeDelta.y + ExtraHeightForBackground;

        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    void UpdateTagFontStyte()
    {
        foreach (LocalizeStringEvent localizeString in LocalizeStringEventList)
        { localizeString.GetComponent<TextMeshProUGUI>().text = SetTagFontStyle(localizeString.GetComponent<TextMeshProUGUI>().text, Style); }
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
            SkillName,
            Description,
            NarrativeDescription,
            EnergyCost,
            Cooldown
        };
    }

    void LocalizeRefresh()
    {
        foreach (LocalizeStringEvent localizeString in LocalizeStringEventList)
        { localizeString.RefreshString(); }
    }

}

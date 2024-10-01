using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3SkillManager : TooltipV3ParentManager
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
    public GameObject PseudoHeader;

    [Title("Dynamic Background Settings")]
    public float ExtraWidthForBackground;
    public float ExtraHeightForBackground;

    [Title("Tooltip Style")]
    public TagFontStyle Style;

    private List<LocalizeStringEvent> LocalizeStringEventList;
    private RectTransform CanvasRect => transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

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
        var backgroundRect = Background.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        backgroundRect.GetWorldCorners(corners);

        // Определяем минимальные и максимальные координаты
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        // Получаем размеры экрана
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        var tooltipTransform = gameObject.GetComponent<RectTransform>();

        // Корректируем положение по X
        if (minX < 0)
        {
            tooltipTransform.position += new Vector3(-minX, 0, 0);
        }
        else if (maxX > screenWidth)
        {
            tooltipTransform.position += new Vector3(screenWidth - maxX, 0, 0);
        }

        // Корректируем положение по Y
        if (minY < 0)
        {
            tooltipTransform.position += new Vector3(0, -minY, 0);
        }
        else if (maxY > screenHeight)
        {
            tooltipTransform.position += new Vector3(0, screenHeight - maxY, 0);
        }
    }

    void EdgeDetection()
    {
        var EndRectItem = PseudoHeader.GetComponent<RectTransform>();
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

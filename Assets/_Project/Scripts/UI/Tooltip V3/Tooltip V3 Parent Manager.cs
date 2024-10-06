using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Components;

public class TooltipV3ParentManager : MonoBehaviour
{
    [Title("Tooltip V3 Parent")]
    
    [Title("Tooltip Style")]
    public TagFontStyle Style;

    [Title("Gameobject Reference")]
    [SerializeField] GameObject TooltipContent;
    [SerializeField] protected GameObject Background;


    [Title("Edge Detecting Settings")]
    //[SerializeField] bool IsCameraSpace = true;
    [SerializeField] float IndentX = 0.1f;
    [SerializeField] float IndentY = 0.1f;

    [Title("Dynamic Background")]
    [Title("Dynamic Background Reference")]
    [SerializeField] protected GameObject PseudoHeader;

    [Title("Dynamic Background Settings")]
    [SerializeField] protected float ExtraWidthForBackground = 200;
    [SerializeField] protected float ExtraHeightForBackground = 200;

    protected List<LocalizeStringEvent> LocalizeStringEventList;
    //private RectTransform CanvasRect => transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    private Camera cameraBrain => FindObjectOfType<Camera>().GetComponent<Camera>();

    #region  ShowHideMethods
    public virtual void ShowTooltip()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public virtual void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    #endregion
    #region AwakeMethods
    protected void Awake()
    {
        TooltipContentShow();
        LocalizeStringEventListInizizalize();
        Refresh();
    }

    protected void Update()
    {
        Refresh();
    }


    protected void TooltipContentShow()
    {
        TooltipContent.gameObject.SetActive(true);
    }

    protected void LocalizeStringEventListInizizalize()
    {
        LocalizeStringEventList = new List<LocalizeStringEvent>
        {
            
        };
    }

    #endregion
    #region RefreshMethods

    protected void Refresh()
    {
        LocalizeRefresh();
        UpdateTagFontStyte();
        ResizeBackground();
        EdgeDetection();

    }
     protected void LocalizeRefresh()
    {
        foreach (LocalizeStringEvent localizeString in LocalizeStringEventList)
        { localizeString.RefreshString(); }
    }

    #region TagFontUpdate
    protected void UpdateTagFontStyte()
    {
        foreach (LocalizeStringEvent localizeString in LocalizeStringEventList)
        { localizeString.GetComponent<TextMeshProUGUI>().text = SetTagFontStyle(localizeString.GetComponent<TextMeshProUGUI>().text, Style); }
    }

    protected string SetTagFontStyle(string text, TagFontStyle style)
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

    protected string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
    }

    #endregion

    protected void ResizeBackground()
    {
        var EndRectItem = PseudoHeader.GetComponent<RectTransform>();
        var width = EndRectItem.sizeDelta.x + ExtraWidthForBackground;
        var height = math.abs(EndRectItem.localPosition.y) + EndRectItem.sizeDelta.y + ExtraHeightForBackground;

        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    protected void EdgeDetection()
    {
        var backgroundRect = Background.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        backgroundRect.GetWorldCorners(corners);

        // Определяем минимальные и максимальные координаты
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        // Получаем размеры экрана в мировых координатах
        Vector3 bottomLeft = cameraBrain.ScreenToWorldPoint(new Vector3(0, 0, cameraBrain.nearClipPlane));
        Vector3 topRight = cameraBrain.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraBrain.nearClipPlane));

        float screenMinX = bottomLeft.x;
        float screenMaxX = topRight.x;
        float screenMinY = bottomLeft.y;
        float screenMaxY = topRight.y;

        var tooltipTransform = gameObject.GetComponent<RectTransform>();

        // Корректируем положение по X
        if (minX < screenMinX + IndentX)
        {
            tooltipTransform.position += new Vector3(screenMinX + IndentX - minX, 0, 0);
        }
        else if (maxX > screenMaxX - IndentX)
        {
            tooltipTransform.position += new Vector3(screenMaxX - IndentX - maxX, 0, 0);
        }

        // Корректируем положение по Y
        if (minY < screenMinY + IndentY)
        {
            tooltipTransform.position += new Vector3(0, screenMinY + IndentY - minY, 0);
        }
        else if (maxY > screenMaxY - IndentY)
        {
            tooltipTransform.position += new Vector3(0, screenMaxY - IndentY - maxY, 0);
        }
    }

    #endregion


}

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TagFontStyleForAllText : MonoBehaviour
{
    [Title("Style")]
    [SerializeField]public bool IsTooltip = true;
    [DisableIf("IsTooltip")] [SerializeField] TagFontStyle Style;
    [SerializeField] int FontStyleNumber = 0;

    private TagFontStyle TooltipStyle;
    private TextMeshProUGUI textMeshPro => GetComponent<TextMeshProUGUI>();
    // Start is called before the first frame update
    void Start()
    {
        if (IsTooltip)
        {
        TooltipStyle = gameObject.GetComponentInParent<TooltipV3ParentManager>().Style;
        DataTransfer(TooltipStyle);
        }
        else
        {
        DataTransfer(Style);
        }
    }

    void DataTransfer(TagFontStyle style)
    {
        var choiceStyle = style.fontStyles[FontStyleNumber];

        textMeshPro.color = choiceStyle.color;

        if (choiceStyle.bold)           { textMeshPro.fontStyle = FontStyles.Bold; }
        if (choiceStyle.italic)         { textMeshPro.fontStyle = FontStyles.Italic; }
        if (choiceStyle.underline)      { textMeshPro.fontStyle = FontStyles.Underline; }
        if (choiceStyle.strikethrough)  { textMeshPro.fontStyle = FontStyles.Strikethrough; }
    }
}

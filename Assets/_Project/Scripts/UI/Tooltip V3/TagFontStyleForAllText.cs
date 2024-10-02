using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TagFontStyleForAllText : MonoBehaviour
{
    [Title("Style")]
    public int StyleNumber = 0;

    private TagFontStyle Style;
    private TextMeshProUGUI textMeshPro => GetComponent<TextMeshProUGUI>();
    // Start is called before the first frame update
    void Start()
    {
        Style = gameObject.GetComponentInParent<TooltipV3ParentManager>().Style;

        DataTransfer();
    }

    void DataTransfer()
    {
        var choiceStyle = Style.fontStyles[StyleNumber];

        textMeshPro.color = choiceStyle.color;

        if (choiceStyle.bold)           { textMeshPro.fontStyle = FontStyles.Bold; }
        if (choiceStyle.italic)         { textMeshPro.fontStyle = FontStyles.Italic; }
        if (choiceStyle.underline)      { textMeshPro.fontStyle = FontStyles.Underline; }
        if (choiceStyle.strikethrough)  { textMeshPro.fontStyle = FontStyles.Strikethrough; }
    }
}

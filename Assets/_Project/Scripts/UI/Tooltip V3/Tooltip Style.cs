using UnityEngine;
using System;
using TMPro;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TooltipStyle", menuName = "FewSecondsManyDeaths/TooltipStyle")]
public class TooltipStyle : ScriptableObject
{

    [Title("Tooltip Panel")]
    public Sprite slicedSprite;
    public Color color = Color.gray;

    [Title("Font")]
    public TMP_FontAsset fontAsset;
    public Color defaultColor = Color.white;

    [Title("Formatting")]
    public Style[] fontStyles;
}

[Serializable]
public class Style
{
    public string tag;
    public Color color;
    public bool bold;
    public bool italic;
    public bool underline;
    public bool strikethrough;
}
using UnityEngine;
using System;
using TMPro;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TooltipStyle", menuName = "FewSecondsManyDeaths/TooltipStyle")]
public class TagFontStyle : ScriptableObject
{
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
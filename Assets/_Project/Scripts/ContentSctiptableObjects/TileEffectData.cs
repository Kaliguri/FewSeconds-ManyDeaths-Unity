using UnityEngine;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "TileEffectData", menuName = "FewSecondsManyDeaths/TileEffectData")]
public class TileEffectData : ScriptableObject
{
    [Header("General")]
    public LocalizedString Name;
    public LocalizedString Description;

    [Header("Icon")]
    public Sprite Icon;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public TileEffectScript TileEffectScript = new TileEffectScript();

}
